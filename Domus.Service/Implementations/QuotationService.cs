using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
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

	public QuotationService(
			IQuotationRepository quotationRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			IUserRepository userRepository,
			IProductDetailRepository productDetailRepository,
			IProductDetailQuotationRepository productDetailQuotationRepository,
			IQuotationNegotiationLogRepository quotationNegotiationLogRepository)
	{
		_quotationRepository = quotationRepository;
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
		_productDetailRepository = productDetailRepository;
		_productDetailQuotationRepository = productDetailQuotationRepository;
		_quotationNegotiationLogRepository = quotationNegotiationLogRepository;
		_mapper = mapper;
	}

    public async Task<ServiceActionResult> CreateQuotation(CreateQuotationRequest request)
    {
		if (!await _userRepository.ExistsAsync(u => u.Id == request.CustomerId))
			throw new UserNotFoundException("Customer not found");
		if (!await _userRepository.ExistsAsync(u => u.Id == request.StaffId))
			throw new UserNotFoundException("Staff not found");

		var customer = await _userRepository.GetAsync(u => u.Id == request.CustomerId);
		var staff = await (await _userRepository.GetAllAsync())
			.Include(u => u.QuotationCreatedByNavigations)
			.FirstOrDefaultAsync(u => u.Id == request.StaffId);

		var negotiationLog = new QuotationNegotiationLog
		{
			IsClosed = false,
			StartAt = DateTime.Now,
		};

		var quotation = new Quotation
		{
			CustomerId = request.CustomerId,
			StaffId = request.StaffId,
			Staff = staff!,
			Customer = customer!,
			QuotationNegotiationLog = negotiationLog,
			CreatedAt = DateTime.Now,
			ExpireAt = DateTime.Now.AddDays(30),
			Status = "Pending",
			IsDeleted = false,
			CreatedBy = request.StaffId,
			CreatedByNavigation = staff!
		};

		await _userRepository.UpdateAsync(staff!);
		// await _quotationNegotiationLogRepository.AddAsync(negotiationLog);
		// await _quotationRepository.AddAsync(quotation);
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
