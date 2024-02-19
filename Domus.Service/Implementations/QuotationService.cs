using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos.Quotations;
using Domus.Domain.Entities;
using Domus.Service.Constants;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;
using Domus.Service.Models.Requests.Quotations;
using Microsoft.EntityFrameworkCore;

namespace Domus.Service.Implementations;

public class QuotationService : IQuotationService
{
	private readonly IQuotationRepository _quotationRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IUserRepository _userRepository;
	private readonly IProductDetailRepository _productDetailRepository;
	private readonly IProductDetailQuotationRepository _productDetailQuotationRepository;
	private readonly IQuotationNegotiationLogRepository _quotationNegotiationLogRepository;
	private readonly INegotiationMessageRepository _negotiationMessageRepository;
	private readonly IServiceRepository _serviceRepository;

	public QuotationService(
			IQuotationRepository quotationRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			IUserRepository userRepository,
			IProductDetailRepository productDetailRepository,
			IProductDetailQuotationRepository productDetailQuotationRepository,
			IServiceRepository serviceRepository,
			INegotiationMessageRepository negotiationMessageRepository,
			IQuotationNegotiationLogRepository quotationNegotiationLogRepository)
	{
		_quotationRepository = quotationRepository;
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
		_productDetailRepository = productDetailRepository;
		_productDetailQuotationRepository = productDetailQuotationRepository;
		_quotationNegotiationLogRepository = quotationNegotiationLogRepository;
		_serviceRepository = serviceRepository;
		_negotiationMessageRepository = negotiationMessageRepository;
		_mapper = mapper;
	}

	public async Task<ServiceActionResult> CreateNegotiationMessage(CreateNegotiationMessageRequest request, Guid quotationId)
	{
		if (!await _quotationRepository.ExistsAsync(q => !q.IsDeleted && q.Id == quotationId))
			throw new QuotationNotFoundException();
		
		var quotationNegotiationLog = await (await _quotationNegotiationLogRepository.GetAllAsync())
			.Include(qnl => qnl.NegotiationMessages)
			.Include(qnl => qnl.Quotation)
			.FirstOrDefaultAsync(qnl => qnl.Quotation.Id == quotationId);

		var negotiationMessage = _mapper.Map<NegotiationMessage>(request);
		if (quotationNegotiationLog == null)
		{
			quotationNegotiationLog = new QuotationNegotiationLog
			{
				QuotationId = quotationId,
				StartAt = DateTime.Now,
				IsClosed = false
			};

			quotationNegotiationLog.NegotiationMessages.Add(negotiationMessage);
			await _quotationNegotiationLogRepository.AddAsync(quotationNegotiationLog);
		}
		else
		{
			quotationNegotiationLog.NegotiationMessages.Add(negotiationMessage);
			await _quotationNegotiationLogRepository.UpdateAsync(quotationNegotiationLog);
		}

		await _unitOfWork.CommitAsync();
		return new ServiceActionResult(true);
     }

    public async Task<ServiceActionResult> CreateQuotation(CreateQuotationRequest request)
    {
		var customer = await _userRepository.GetAsync(u => u.Id == request.CustomerId);
		if (customer == null)
			throw new UserNotFoundException("Customer not found");
		if (!await _userRepository.ExistsAsync(u => u.Id == request.StaffId))
			throw new UserNotFoundException("Staff not found");

		var quotation = new Quotation
		{
			CustomerId = request.CustomerId,
			StaffId = request.StaffId,
			CreatedBy = request.CustomerId,
			CreatedAt = DateTime.Now,
			ExpireAt = DateTime.Now.AddDays(30),
			Status = QuotationStatusConstants.Requested,
			IsDeleted = false,
		};

		foreach (var productDetailId in request.ProductDetails)
		{
			var productDetailEntity = await _productDetailRepository.GetAsync(pd => pd.Id == productDetailId);
			if (productDetailEntity == null)
				throw new ProductDetailNotFoundException();

			var productDetailQuotation = new ProductDetailQuotation
			{
				ProductDetailId = productDetailId,
				QuotationId = quotation.Id,
				Quantity = 1,
				Price = productDetailEntity.DisplayPrice,
				MonetaryUnit = "USD",
				QuantityType = "Unit",
			};

			quotation.ProductDetailQuotations.Add(productDetailQuotation);
		}

		foreach (var serviceId in request.Services)
		{
			var serviceEntity = await _serviceRepository.GetAsync(pd => pd.Id == serviceId);
			if (serviceEntity == null)
				throw new ServiceNotFoundException();

			quotation.Services.Add(serviceEntity);
		}

		customer.QuotationCreatedByNavigations.Add(quotation);
		await _userRepository.UpdateAsync(customer);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> DeleteQuotation(Guid id)
    {
		var quotation = await _quotationRepository.GetAsync(q => q.Id == id);
		if (quotation == null)
			throw new QuotationNotFoundException();
		
		quotation.IsDeleted = true;
		await _quotationRepository.UpdateAsync(quotation);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> GetAllNegotiationMessages(Guid quotationId)
    {
		var queryableNegotiationMessages = (await _negotiationMessageRepository.FindAsync(m => m.QuotationNegotiationLog.QuotationId == quotationId))
			.ProjectTo<DtoNegotiationMessage>(_mapper.ConfigurationProvider);

		return new ServiceActionResult(true) { Data = queryableNegotiationMessages };
    }

    public async Task<ServiceActionResult> GetAllQuotations()
    {
		var quotations = await (await _quotationRepository.GetAllAsync())
			.ProjectTo<DtoQuotation>(_mapper.ConfigurationProvider)
			.ToListAsync();

		foreach (var quotation in quotations)
		{
			var products = await (await _productDetailQuotationRepository.GetAllAsync()).Where(pdq => pdq.QuotationId == quotation.Id).ToListAsync();

			quotation.TotalPrice = (float)products.Sum(pdq => pdq.Price * pdq.Quantity);
		}

		return new ServiceActionResult(true) { Data = quotations };
    }

    public async Task<ServiceActionResult> GetPaginatedNegotiationMessages(BasePaginatedRequest request, Guid quotationId)
    {
		var queryableNegotiationMessages = (await _negotiationMessageRepository.FindAsync(m => m.QuotationNegotiationLog.QuotationId == quotationId))
			.ProjectTo<DtoNegotiationMessage>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(queryableNegotiationMessages, request.PageSize, request.PageIndex);

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> GetPaginatedQuotations(BasePaginatedRequest request)
    {
		var queryableQuotations = (await _quotationRepository.GetAllAsync()).ProjectTo<DtoQuotation>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(queryableQuotations, request.PageSize, request.PageIndex);
		var quotationList = new List<DtoQuotation>();
		
		foreach (var quotation in await ((IQueryable<DtoQuotation>)paginatedResult.Items!).ToListAsync())
		{
			var products = await (await _productDetailQuotationRepository.GetAllAsync()).Where(pdq => pdq.QuotationId == quotation.Id).ToListAsync();
			
			quotation.TotalPrice = (float)products.Sum(pdq => pdq.Price * pdq.Quantity);
			quotationList.Add(quotation);
		}

		paginatedResult.Items = quotationList;

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> GetQuotationById(Guid id)
    {
		var quotation = (await _quotationRepository.GetAllAsync())
			.Where(q => q.Id == id)
			.ProjectTo<DtoQuotationFullDetails>(_mapper.ConfigurationProvider)
			.FirstOrDefault() ?? throw new QuotationNotFoundException();

		return new ServiceActionResult(true) { Data = quotation };
    }

    public async Task<ServiceActionResult> SearchQuotations(SearchUsingGetRequest request)
    {
		var quotations = await (await _quotationRepository.FindAsync(p => !p.IsDeleted))
		    .ProjectTo<DtoQuotationFullDetails>(_mapper.ConfigurationProvider)
		    .ToListAsync();

		foreach (var quotation in quotations)
		{
			var products = await (await _productDetailQuotationRepository.GetAllAsync()).Where(pdq => pdq.QuotationId == quotation.Id).ToListAsync();
			
			quotation.TotalPrice = (float)products.Sum(pdq => pdq.Price * pdq.Quantity);
		}
	    
	    if (!string.IsNullOrEmpty(request.SearchField))
	    {
			quotations = quotations 
				.Where(p => ReflectionHelper.GetStringValueByName(typeof(DtoQuotationFullDetails), request.SearchField, p).Contains(request.SearchValue ?? string.Empty, StringComparison.OrdinalIgnoreCase))
				.ToList();
	    }

	    if (!string.IsNullOrEmpty(request.SortField))
	    {
			Expression<Func<DtoQuotationFullDetails, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(DtoQuotationFullDetails), request.SortField, p);
			quotations = request.Descending
				? quotations.OrderByDescending(orderExpr.Compile()).ToList()
				: quotations.OrderBy(orderExpr.Compile()).ToList();
	    }

	    var paginatedResult = PaginationHelper.BuildPaginatedResult(quotations, request.PageSize, request.PageIndex);

	    return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> UpdateQuotation(UpdateQuotationRequest request, Guid id)
    {
		var quotation = await _quotationRepository.GetAsync(q => q.Id == id);
		if (quotation == null)
			throw new QuotationNotFoundException();

		_mapper.Map(request, quotation);
		quotation.LastUpdatedAt = DateTime.Now;

		await _quotationRepository.UpdateAsync(quotation);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }
}
