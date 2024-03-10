using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Constants;
using Domus.Service.Enums;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Common;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Contracts;
using Domus.Service.Models.Requests.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnauthorizedAccessException = Domus.Service.Exceptions.UnauthorizedAccessException;

namespace Domus.Service.Implementations;

public class ContractService : IContractService
{
    private readonly IQuotationRepository _quotationRepository;
    private readonly IFileService _fileService;
    private readonly IQuotationRevisionRepository _quotationRevisionRepository;
    private readonly UserManager<DomusUser> _userManager;
    private readonly IContractRepository _contractRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ContractService(IContractRepository contractRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository, IQuotationRepository quotationRepository, IQuotationRevisionRepository quotationRevisionRepository, IFileService fileService, UserManager<DomusUser> userManager)
    {
        _contractRepository = contractRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepository = userRepository;
        _quotationRepository = quotationRepository;
        _quotationRevisionRepository = quotationRevisionRepository;
        _fileService = fileService;
        _userManager = userManager;
    }
    
    public async Task<ServiceActionResult> GetAllContracts()
    {
        var contracts = (await _contractRepository.GetAllAsync()).Where(x => !x.IsDeleted).ProjectTo<DtoContract>(_mapper.ConfigurationProvider);
        return new ServiceActionResult(true)
        {
            Data = contracts
        };
    }

    public async Task<ServiceActionResult> GetPaginatedContracts(BasePaginatedRequest request)
    {
        var dtoPackages = (await _contractRepository.GetAllAsync()).Where(pk => !pk.IsDeleted).ProjectTo<DtoContract>(_mapper.ConfigurationProvider);
        var paginatedList = PaginationHelper.BuildPaginatedResult(dtoPackages.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = paginatedList
        };
    }

    public async Task<ServiceActionResult> GetContract(Guid ContractId)
    {
        var contract = await (await _contractRepository.FindAsync((x => !x.IsDeleted && x.Id == ContractId)))
            .ProjectTo<DtoContract>(_mapper.ConfigurationProvider).FirstOrDefaultAsync() ?? throw new Exception("Contract Not Found");
        return new ServiceActionResult(true)
        {
            Data = contract
        };
    }

    public async Task<ServiceActionResult> CreateContract(ContractRequest request)
    {
        await ValidateContractRequest(request);
        var contract = _mapper.Map<Contract>(request);
        contract.Status = ContractStatus.SENT;
        await _contractRepository.AddAsync(contract);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> UpdateContract(ContractRequest request, Guid ContractId)
    {
        await ValidateContractRequest(request);

        var contract = await _contractRepository.GetAsync(x => x.Id == ContractId && !x.IsDeleted) ?? throw new Exception("Contract Not Found");
        contract.Name = request.Name ?? contract.Name;
        contract.Description = request.Description ?? contract.Description;
        contract.Notes = request.Notes ?? contract.Notes;
        contract.Attachments = request.Attachments ?? contract.Attachments;
        contract.Signature = request.Signature ?? contract.Signature;
        contract.ClientId = request.ClientId ?? contract.ClientId;
        contract.ContractorId = request.ContractorId ?? contract.ContractorId;
        await _contractRepository.UpdateAsync(contract);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> DeleteContract(Guid ContractId)
    {
        var contract = (await _contractRepository.GetAsync(x => !x.IsDeleted && x.Id == ContractId)) ?? throw new Exception("Contract Not Found");
        contract.IsDeleted = true;
        await _contractRepository.UpdateAsync(contract);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> GetContractByClientId(string clientId)
    {
        if (!_userRepository.Exists(x => x.Id == clientId))
        {
            throw new Exception("Not Found Client");
        }

        var contracts = (await _contractRepository.FindAsync(x => !x.IsDeleted && x.ClientId.Equals(clientId))).ProjectTo<DtoContract>(_mapper.ConfigurationProvider) ;
        return new ServiceActionResult(true)
        {
            Data = contracts
        };
    }

    public async Task<ServiceActionResult> GetContractByContractorId(string contractorId)
    {
        if (!await _userRepository.ExistsAsync(x => x.Id == contractorId))
        {
            throw new Exception("Not Found Contractor");
        }
        var contract = (await _contractRepository.FindAsync(x => !x.IsDeleted && x.ContractorId.Equals(contractorId))).ProjectTo<DtoContract>(_mapper.ConfigurationProvider);
        return new ServiceActionResult(true)
        {
            Data = contract
        };
    }

    public async Task<ServiceActionResult> SearchContracts(BaseSearchRequest request)
    {
        var contracts = (await _contractRepository.FindAsync(c => !c.IsDeleted)).ToList();
        
        foreach (var searchInfo in request.DisjunctionSearchInfos)
        {
            contracts = contracts
                .Where(p => ReflectionHelper.GetStringValueByName(typeof(Contract), searchInfo.FieldName, p)
                    .Contains(searchInfo.Keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (request.ConjunctionSearchInfos.Any())
        {
            var initialSearchInfo = request.ConjunctionSearchInfos.First();
            Expression<Func<Contract, bool>> conjunctionWhere = p => ReflectionHelper.GetStringValueByName(typeof(Contract), initialSearchInfo.FieldName, p)
                .Contains(initialSearchInfo.Keyword, StringComparison.OrdinalIgnoreCase);
            
            foreach (var (searchInfo, i) in request.ConjunctionSearchInfos.Select((value, i) => (value, i)))
            {
                if (i == 0)
                    continue;
                
                Expression<Func<Contract, bool>> whereExpr = p => ReflectionHelper.GetStringValueByName(typeof(Contract), searchInfo.FieldName, p)
                    .Contains(searchInfo.Keyword, StringComparison.OrdinalIgnoreCase);
                conjunctionWhere = ExpressionHelper.CombineOrExpressions(conjunctionWhere, whereExpr);
            }

            contracts = contracts.Where(conjunctionWhere.Compile()).ToList();
        }

        if (request.SortInfos.Any())
        {
            request.SortInfos = request.SortInfos.OrderBy(si => si.Priority).ToList();
            var initialSortInfo = request.SortInfos.First();
            Expression<Func<Contract, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(Contract), initialSortInfo.FieldName, p);

            contracts = initialSortInfo.Descending ? contracts.OrderByDescending(orderExpr.Compile()).ToList() : contracts.OrderBy(orderExpr.Compile()).ToList();
            
            foreach (var (sortInfo, i) in request.SortInfos.Select((value, i) => (value, i)))
            {
                if (i == 0)
                    continue;
                
                orderExpr = p => ReflectionHelper.GetValueByName(typeof(Contract), sortInfo.FieldName, p);
                contracts = sortInfo.Descending ? contracts.OrderByDescending(orderExpr.Compile()).ToList() : contracts.OrderBy(orderExpr.Compile()).ToList();
            }
        }

        var paginatedResult = PaginationHelper.BuildPaginatedResult<Contract, DtoContract>(_mapper, contracts, request.PageSize, request.PageIndex);

        return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> SearchContractsUsingGet(SearchUsingGetRequest request)
    {
        var contracts = await (await _contractRepository.FindAsync(p => !p.IsDeleted))
            .ProjectTo<DtoContract>(_mapper.ConfigurationProvider)
            .ToListAsync();
      
        
        if (!string.IsNullOrEmpty(request.SearchField))
        {
            contracts = contracts
                .Where(p => ReflectionHelper.GetStringValueByName(typeof(DtoContract), request.SearchField, p).Contains(request.SearchValue ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (!string.IsNullOrEmpty(request.SortField))
        {
            Expression<Func<DtoContract, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(DtoContract), request.SortField, p);
            contracts = request.Descending
                ? contracts.OrderByDescending(orderExpr.Compile()).ToList()
                : contracts.OrderBy(orderExpr.Compile()).ToList();
        }

        var paginatedResult = PaginationHelper.BuildPaginatedResult(contracts, request.PageSize, request.PageIndex);
        var finalProducts = (IEnumerable<DtoContract>)paginatedResult.Items!;

        paginatedResult.Items = finalProducts;

        return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> DeleteContracts(List<Guid> ContractIds)
    {
        var contracts = new List<Contract>();

        foreach (var contractId in ContractIds)
        {
            var contract = await _contractRepository.GetAsync(y => y.Id == contractId) ?? throw new Exception($"Not Found Contract: {contractId}");
            contract.IsDeleted = true;
            contracts.Add(contract); 
        }

        await _contractRepository.UpdateManyAsync(contracts);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> SignContract(Guid contractId, IFormFile signature)
    {
        var contract = await _contractRepository.GetAsync(x => x.Id == contractId) ??
                       throw new Exception("Contract Not Found");
        contract.Signature = await _fileService.UploadFile(signature);
        contract.Status = ContractStatus.SIGNED;
        await _contractRepository.UpdateAsync(contract);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }

    private async Task ValidateContractRequest(ContractRequest request)
    {
        if (!await _quotationRevisionRepository.ExistsAsync(x => x.Id == request.QuotationRevisionId && !x.IsDeleted && !x.Quotation.IsDeleted))
            throw new Exception($"Not found quotation revision: {request.QuotationRevisionId}");
        
        var clientUser = await _userRepository.GetAsync(x => x.Id.Equals(request.ClientId)&& !x.IsDeleted) ??
                         throw new Exception($"Not found Client: {request.ClientId}");
        var clientRoles = await _userManager.GetRolesAsync(clientUser);
        if (clientRoles.Contains(UserRoleConstants.STAFF))
            throw new UnauthorizedAccessException($"Unauthorized ContractorId: {request.ClientId}");
        
        var contractorUser = await _userRepository.GetAsync(x => x.Id.Equals(request.ContractorId)&& !x.IsDeleted) ??
                             throw new Exception($"Not found Contractor: {request.ContractorId}");
        var contractorRoles = await _userManager.GetRolesAsync(contractorUser);
        if (!contractorRoles.Contains(UserRoleConstants.STAFF))
            throw new UnauthorizedAccessException($"Unauthorized ContractorId: {request.ContractorId}");
    }
}