using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Constants;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
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
	private readonly IServiceRepository _serviceRepository;
	// private readonly INegotiationMessageRepository _negotiationMessageRespository;

	public QuotationService(
			IQuotationRepository quotationRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			IUserRepository userRepository,
			IProductDetailRepository productDetailRepository,
			IProductDetailQuotationRepository productDetailQuotationRepository,
			// INegotiationMessageRepository negotiationMessageRespository,
			IServiceRepository serviceRepository,
			IQuotationNegotiationLogRepository quotationNegotiationLogRepository)
	{
		_quotationRepository = quotationRepository;
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
		_productDetailRepository = productDetailRepository;
		_productDetailQuotationRepository = productDetailQuotationRepository;
		_quotationNegotiationLogRepository = quotationNegotiationLogRepository;
		// _negotiationMessageRespository = negotiationMessageRespository;
		_serviceRepository = serviceRepository;
		_mapper = mapper;
	}

    public async Task<ServiceActionResult> CreateNegotiationMessage(CreateNegotiationMessageRequest request, Guid id)
    {
		var quotation = (await _quotationRepository.FindAsync(q => q.Id == id && !q.IsDeleted))
			.Include(q => q.QuotationNegotiationLog)
			.ThenInclude(qnl => qnl.NegotiationMessages)
			.FirstOrDefault();
		if (quotation == null)
			throw new QuotationNotFoundException();

		if (quotation.QuotationNegotiationLog == null)
			quotation.QuotationNegotiationLog = new QuotationNegotiationLog
			{
				QuotationId = quotation.Id,
				IsClosed = false,
				StartAt = DateTime.Now,
				CloseAt = null
			};

		var message = _mapper.Map<NegotiationMessage>(request);
		quotation.QuotationNegotiationLog.NegotiationMessages.Add(message);
		await _quotationRepository.UpdateAsync(quotation);
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

    public async Task<ServiceActionResult> GetAllQuotations()
    {
		var quotations = await _quotationRepository.GetAllAsync();

		return new ServiceActionResult(true) { Data = _mapper.Map<IEnumerable<DtoQuotation>>(quotations) };
    }

    public async Task<ServiceActionResult> GetPaginatedQuotations(BasePaginatedRequest request)
    {
		var queryableQuotations = (await _quotationRepository.GetAllAsync()).ProjectTo<DtoQuotation>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(queryableQuotations, request.PageSize, request.PageIndex);

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> GetQuotationById(Guid id)
    {
		var quotation = await _quotationRepository.GetAsync(q => q.Id == id);
		if (quotation == null)
			throw new QuotationNotFoundException();

		return new ServiceActionResult(true) { Data = _mapper.Map<DtoQuotation>(quotation) };
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
