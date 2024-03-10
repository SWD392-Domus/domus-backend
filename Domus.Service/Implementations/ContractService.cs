using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Enums;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Contracts;
using Domus.Service.Models.Requests.Products;

namespace Domus.Service.Implementations;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ContractService(IContractRepository contractRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository)
    {
        _contractRepository = contractRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepository = userRepository;
    }
    
    public async Task<ServiceActionResult> GetAllContracts()
    {
        var contracts = (await _contractRepository.GetAllAsync()).Where(x => !x.IsDeleted).ProjectTo<DtoContract>(_mapper.ConfigurationProvider);
        return new ServiceActionResult(true)
        {
            Data = contracts
        };
    }

    public Task<ServiceActionResult> GetPaginatedContracts(BasePaginatedRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> GetContract(Guid ContractId)
    {
        var contract = _mapper.Map<DtoContract>(await _contractRepository.GetAsync(x => !x.IsDeleted && x.Id == ContractId)) ?? throw new Exception("Contract Not Found");
        return new ServiceActionResult(true)
        {
            Data = contract
        };
    }

    public async Task<ServiceActionResult> CreateContract(ContractRequest request)
    {
        var contract = _mapper.Map<Contract>(request);
        contract.Status = ContractStatus.WAITING;
        await _contractRepository.AddAsync(contract);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> UpdateContract(ContractRequest request, Guid ContractId)
    {
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

    public Task<ServiceActionResult> SearchContracts(BaseSearchRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> SearchContractsUsingGet(SearchUsingGetRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> DeleteContracts(List<Guid> ContractIds)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> SignContract(Guid contractId, string signature)
    {
        var contract = await _contractRepository.GetAsync(x => x.Id == contractId) ??
                       throw new Exception("Contract Not Found");
        contract.Signature = signature;
        contract.Status = ContractStatus.COMPLETED;
        return new ServiceActionResult(true);
    }
}