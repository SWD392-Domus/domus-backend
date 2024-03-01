using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos.Products;
using Domus.Domain.Dtos.Quotations;
using Domus.Domain.Entities;
using Domus.Service.Constants;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;
using Domus.Service.Models.Requests.Quotations;
using Microsoft.AspNetCore.Identity;
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
	private readonly IJwtService _jwtService;
	private readonly UserManager<DomusUser> _userManager;
	private readonly IPackageRepository _packageRepository;
	private readonly IServiceQuotationRepository _serviceQuotationRepository;

	public QuotationService(
			IQuotationRepository quotationRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			IUserRepository userRepository,
			IProductDetailRepository productDetailRepository,
			IProductDetailQuotationRepository productDetailQuotationRepository,
			IServiceRepository serviceRepository,
			INegotiationMessageRepository negotiationMessageRepository,
			IQuotationNegotiationLogRepository quotationNegotiationLogRepository,
			IJwtService jwtService, 
			UserManager<DomusUser> userManager,
			IPackageRepository packageRepository,
			IServiceQuotationRepository serviceQuotationRepository)
	{
		_quotationRepository = quotationRepository;
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
		_productDetailRepository = productDetailRepository;
		_productDetailQuotationRepository = productDetailQuotationRepository;
		_quotationNegotiationLogRepository = quotationNegotiationLogRepository;
		_jwtService = jwtService;
		_userManager = userManager;
		_packageRepository = packageRepository;
		_serviceRepository = serviceRepository;
		_negotiationMessageRepository = negotiationMessageRepository;
		_serviceQuotationRepository = serviceQuotationRepository;
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

    public async Task<ServiceActionResult> CreateQuotation(CreateQuotationRequest request, string token)
    {
	    var isValidToken = _jwtService.IsValidToken(token);
	    if (!isValidToken)
		    throw new InvalidTokenException();
	    
	    if (!await _packageRepository.ExistsAsync(p => !p.IsDeleted && p.Id == request.PackageId))
		    throw new PackageNotFoundException();
	    
	    var userId = _jwtService.GetTokenClaim(token, TokenClaimConstants.SUBJECT)?.ToString() ?? string.Empty;
	    var creator = await _userRepository.GetAsync(u => u.Id == userId) ?? throw new UserNotFoundException("Creator not found");
		var creatorRoles = await _userManager.GetRolesAsync(creator);
		var createdByStaff = creatorRoles.Contains(UserRoleConstants.STAFF);

		var quotation = new Quotation
		{
			CustomerId = createdByStaff ? "82542982-e958-4817-9a69-2fb8382df1f6" : userId,
			StaffId = createdByStaff ? userId : "c713aacc-3582-4598-8670-22590d837179",
			CreatedBy = userId,
			CreatedAt = DateTime.Now,
			ExpireAt = request.ExpireAt == null ? request.ExpireAt : DateTime.Now.AddDays(30),
			Status = QuotationStatusConstants.Requested,
			IsDeleted = false,
			PackageId = request.PackageId
		};

		foreach (var productDetail in request.ProductDetails)
		{
			var productDetailEntity = await _productDetailRepository.GetAsync(pd => pd.Id == productDetail.Id);
			if (productDetailEntity == null)
				throw new ProductDetailNotFoundException();

			var productDetailQuotation = new ProductDetailQuotation
			{
				ProductDetailId = productDetail.Id,
				QuotationId = quotation.Id,
				Quantity = productDetail.Quantity,
				Price = productDetailEntity.DisplayPrice,
				MonetaryUnit = "USD",
				QuantityType = "Unit",
			};

			var productDetailQuotationRevision = new ProductDetailQuotationRevision
			{
				ProductDetailQuotation = productDetailQuotation,
				Price = productDetailQuotation.Price,
				Version = 0
			};

			productDetailQuotation.ProductDetailQuotationRevisions.Add(productDetailQuotationRevision);
			quotation.ProductDetailQuotations.Add(productDetailQuotation);
		}

		// Todo: change to ServiceQuotation
		// foreach (var serviceId in request.Services)
		// {
		// 	var serviceEntity = await _serviceRepository.GetAsync(pd => pd.Id == serviceId);
		// 	if (serviceEntity == null)
		// 		throw new ServiceNotFoundException();
		//
		// 	quotation.Services.Add(serviceEntity);
		// }

		await _quotationRepository.AddAsync(quotation);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> DeleteMultipleQuotations(IEnumerable<Guid> ids)
    {
	    await _quotationRepository.DeleteManyAsync(p => !p.IsDeleted && ids.Contains(p.Id));
	    await _unitOfWork.CommitAsync();
	    
	    return new ServiceActionResult(true) { Detail = "Quotations deleted successfully"};
    }

    public async Task<ServiceActionResult> DeleteQuotation(Guid id)
    {
		var quotation = await _quotationRepository.GetAsync(q => !q.IsDeleted && q.Id == id) ?? throw new QuotationNotFoundException();
		
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
		var quotations = await (await _quotationRepository.FindAsync(q => !q.IsDeleted))
			.OrderByDescending(q => q.CreatedAt)
			.ProjectTo<DtoQuotation>(_mapper.ConfigurationProvider)
			.ToListAsync();

		foreach (var quotation in quotations)
		{
			quotation.TotalPrice = (float)await GetQuotationTotalPrice(quotation.Id);
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
		var queryableQuotations = (await _quotationRepository.FindAsync(q => !q.IsDeleted))
			.OrderByDescending(q => q.CreatedAt)
			.ProjectTo<DtoQuotation>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(queryableQuotations, request.PageSize, request.PageIndex);
		var quotationList = new List<DtoQuotation>();
		
		foreach (var quotation in await ((IQueryable<DtoQuotation>)paginatedResult.Items!).ToListAsync())
		{
			quotation.TotalPrice = (float)await GetQuotationTotalPrice(quotation.Id);
			quotationList.Add(quotation);
		}

		paginatedResult.Items = quotationList;

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> GetQuotationById(Guid id)
    {
		var quotation = (await _quotationRepository.FindAsync(q => !q.IsDeleted && q.Id == id))
			.ProjectTo<DtoQuotationFullDetails>(_mapper.ConfigurationProvider)
			.FirstOrDefault() ?? throw new QuotationNotFoundException();
		
		var products = (await _productDetailQuotationRepository.GetAllAsync())
			.Include(pdq => pdq.ProductDetailQuotationRevisions)
			.Include(pdq => pdq.ProductDetail)
			.ThenInclude(pd => pd.Product)
			.Where(pdq => pdq.QuotationId == quotation.Id)
			.ToList()
			.Select(pdq => 
			{
				var latestRevision = pdq.ProductDetailQuotationRevisions.MaxBy(r => r.Version);
				pdq.Price = latestRevision?.Price ?? pdq.Price;
				pdq.Quantity = latestRevision?.Quantity ?? pdq.Quantity;
				return pdq;
			});
		quotation.ProductDetailQuotations = _mapper.Map<ICollection<DtoProductDetailQuotation>>(products);
		
		var totalProductPrice = products.Select(pdq => pdq.ProductDetailQuotationRevisions.MaxBy(r => r.Version))
			.Sum(r => (float)(r?.Price ?? 0) * r?.Quantity ?? 0);
		var totalServicePrice = quotation.ServiceQuotations.Sum(s => s.Price);
		quotation.TotalPrice = totalProductPrice + totalServicePrice;

		return new ServiceActionResult(true) { Data = quotation };
    }

    public async Task<ServiceActionResult> SearchQuotations(SearchUsingGetRequest request)
    {
		var quotations = await (await _quotationRepository.FindAsync(p => !p.IsDeleted))
			.OrderByDescending(p => p.CreatedAt)
		    .ProjectTo<DtoQuotationFullDetails>(_mapper.ConfigurationProvider)
		    .ToListAsync();
		
		foreach (var quotation in quotations)
		{
			var products = (await _productDetailQuotationRepository.GetAllAsync())
				.Where(pdq => pdq.QuotationId == quotation.Id)
				.Include(pdq => pdq.ProductDetailQuotationRevisions)
				.Include(pdq => pdq.ProductDetail)
				.ThenInclude(pd => pd.Product)
				.AsSplitQuery()
				.ToList()
				.Select(pdq => 
				{
					var latestRevision = pdq.ProductDetailQuotationRevisions.MaxBy(r => r.Version);
					pdq.Price = latestRevision?.Price ?? pdq.Price;
					pdq.Quantity = latestRevision?.Quantity ?? pdq.Quantity;
					return pdq;
				});
			quotation.ProductDetailQuotations = _mapper.Map<ICollection<DtoProductDetailQuotation>>(products);
			var totalProductPrice = products.Select(pdq => pdq.ProductDetailQuotationRevisions.MaxBy(r => r.Version))
				.Sum(r => (float)(r?.Price ?? 0) * r?.Quantity ?? 0);
			var totalServicePrice = quotation.ServiceQuotations.Sum(s => s.Price);
			quotation.TotalPrice = totalProductPrice + totalServicePrice;
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
	    if (!await _userRepository.ExistsAsync(u => u.Id == request.CustomerId))
		    throw new UserNotFoundException("Customer not found");
	    if (!await _userRepository.ExistsAsync(u => u.Id == request.StaffId))
		    throw new UserNotFoundException("Staff not found");
	    
		var quotation = await (await _quotationRepository.FindAsync(q => !q.IsDeleted && q.Id == id))
			.Include(q => q.ServiceQuotations)
			.Include(q => q.ProductDetailQuotations)
			.FirstOrDefaultAsync() ?? throw new QuotationNotFoundException();

		_mapper.Map(request, quotation);
		
		foreach (var requestService in request.Services)
		{
			if (!await _serviceRepository.ExistsAsync(s => s.Id == requestService.ServiceId))
				throw new ServiceNotFoundException();
			var serviceQuotation = await _serviceQuotationRepository.GetAsync(s => s.ServiceId == requestService.ServiceId && s.QuotationId == quotation.Id);
			if (serviceQuotation == null)
			{
				var newServiceQuotation = new ServiceQuotation
				{
					QuotationId = quotation.Id,
					ServiceId = requestService.ServiceId,
					Price = requestService.Price
				};

				quotation.ServiceQuotations.Add(newServiceQuotation);
				continue;
			}

			quotation.ServiceQuotations.Remove(serviceQuotation);
			serviceQuotation.Price = requestService.Price;
			quotation.ServiceQuotations.Add(serviceQuotation);
		}

		var excludedServices = new List<ServiceQuotation>(quotation.ServiceQuotations.Where(s => !request.Services.Select(rs => rs.ServiceId).Contains(s.ServiceId)));
		foreach (var excludedService in excludedServices)
			quotation.ServiceQuotations.Remove(excludedService);
		
		foreach (var requestProductDetail in request.ProductDetailQuotations)
		{
			if (!await _productDetailRepository.ExistsAsync(x => x.Id == requestProductDetail.ProductDetailId))
				throw new ProductDetailNotFoundException();
			var productDetail = await (await _productDetailQuotationRepository.FindAsync(s => s.ProductDetailId == requestProductDetail.ProductDetailId && s.QuotationId == quotation.Id))
				.Include(s => s.ProductDetailQuotationRevisions)
				.FirstOrDefaultAsync();
			
			if (productDetail == null)
			{
				var newProductDetail = new ProductDetailQuotation
				{	
					ProductDetailId = requestProductDetail.ProductDetailId,
					QuotationId = quotation.Id,
					Quantity = requestProductDetail.Quantity,
					Price = requestProductDetail.Price,
					MonetaryUnit = string.IsNullOrEmpty(requestProductDetail.MonetaryUnit) ? "USD" : requestProductDetail.MonetaryUnit,
					QuantityType = string.IsNullOrEmpty(requestProductDetail.QuantityType) ? "Unit" : requestProductDetail.QuantityType
				};
				
				var productDetailQuotationRevision = new ProductDetailQuotationRevision
				{
					ProductDetailQuotation = newProductDetail,
					Price = requestProductDetail.Price,
					Quantity = requestProductDetail.Quantity,
					Version = 0
				};
				
				newProductDetail.ProductDetailQuotationRevisions.Add(productDetailQuotationRevision);
				quotation.ProductDetailQuotations.Add(newProductDetail);
				continue;
			};
			
			var newProductDetailQuotationRevision = new ProductDetailQuotationRevision
			{
				ProductDetailQuotationId = productDetail.Id,
				Price = requestProductDetail.Price,
				Quantity = requestProductDetail.Quantity,
				Version = productDetail.ProductDetailQuotationRevisions.Count
			};

			productDetail.ProductDetailQuotationRevisions.Add(newProductDetailQuotationRevision);
		}
		
		var excludedProductDetails = new List<ProductDetailQuotation>(quotation.ProductDetailQuotations.Where(s => !request.ProductDetailQuotations.Select(pdq => pdq.ProductDetailId).Contains(s.ProductDetailId)));
		foreach (var excludedProductDetail in excludedProductDetails)
			quotation.ProductDetailQuotations.Remove(excludedProductDetail);

		quotation.LastUpdatedAt = DateTime.Now;
		await _quotationRepository.UpdateAsync(quotation);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    private async Task<double> GetQuotationTotalPrice(Guid quotationId)
    {
	    var products = (await _productDetailQuotationRepository.GetAllAsync())
		    .Include(pdq => pdq.ProductDetailQuotationRevisions)
		    .Include(pdq => pdq.ProductDetail)
		    .ThenInclude(pd => pd.Product)
		    .Where(pdq => pdq.QuotationId == quotationId)
		    .ToList()
		    .Select(pdq => 
		    {
			    var latestRevision = pdq.ProductDetailQuotationRevisions.MaxBy(r => r.Version);
			    pdq.Price = latestRevision?.Price ?? pdq.Price;
			    pdq.Quantity = latestRevision?.Quantity ?? pdq.Quantity;
			    return pdq;
		    });
	    var services = await (await _serviceQuotationRepository.FindAsync(s => s.QuotationId == quotationId))
		    .ToListAsync();
		
	    var totalProductPrice = products.Select(pdq => pdq.ProductDetailQuotationRevisions.MaxBy(r => r.Version))
		    .Sum(r => (float)(r?.Price ?? r?.ProductDetailQuotation.Price ?? 0) * r?.Quantity ?? r?.ProductDetailQuotation.Quantity ?? 0);
	    
	    var totalServicePrice = services.Sum(s => s.Price);
	    return totalProductPrice + totalServicePrice;
    }
}
