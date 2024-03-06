using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Dtos.Products;
using Domus.Domain.Entities;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.ProductDetails;
using Domus.Service.Models.Requests.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace Domus.Service.Implementations;

public class ProductDetailService : IProductDetailService
{
	private readonly IProductDetailRepository _productDetailRepository;
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IProductAttributeRepository _productAttributeRepository;
	private readonly IFileService _fileService;

	public ProductDetailService(
			IProductDetailRepository productDetailRepository,
			IMapper mapper,
			IUnitOfWork unitOfWork,
			IProductRepository productRepository,
			IFileService fileService,
			IProductAttributeRepository productAttributeRepository)
	{
		_productDetailRepository = productDetailRepository;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_productRepository = productRepository;
		_productAttributeRepository = productAttributeRepository;
		_fileService = fileService;
	}

    public async Task<ServiceActionResult> CreateProductDetail(CreateProductDetailRequest request)
    {
		var product = await _productRepository.GetAsync(p => p.Id == request.ProductId);
		if (product == null)
			throw new ProductNotFoundException();

		var productDetail = _mapper.Map<ProductDetail>(request);

		var productPrice = new ProductPrice
		{
			ProductDetailId = productDetail.Id,
			Price = request.Price,
			MonetaryUnit = request.MonetaryUnit,
			Quantity = request.Quantity,
			QuantityType = request.QuantityType
		};
		
		var productAttributeValues = new List<ProductAttributeValue>();
		foreach (var productAttributeValue in request.Attributes)
		{
			var productAttribute = await _productAttributeRepository.GetAsync(pa => pa.AttributeName == productAttributeValue.Name);
			if (productAttribute == null)
			{
				productAttribute = new ProductAttribute
				{
					AttributeName = productAttributeValue.Name
				};
			}

			var productAttributeValueEntity = new ProductAttributeValue
			{
				ProductAttributeId = productAttribute.Id,
				ProductAttribute = productAttribute,
				Value = productAttributeValue.Value,
				ValueType = productAttributeValue.ValueType
			};

			productAttributeValues.Add(productAttributeValueEntity);
		}

		product.ProductDetails.Add(productDetail);
		productDetail.ProductPrices.Add(productPrice);
		productDetail.ProductAttributeValues = productAttributeValues;
		await _productRepository.UpdateAsync(product);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true) { Data = _mapper.Map<DtoProductDetail>(productDetail) };
    }

    public async Task<ServiceActionResult> DeleteProductDetail(Guid id)
    {
		var productDetail = await _productDetailRepository.GetAsync(pd => !pd.IsDeleted && pd.Id == id) ?? throw new ProductDetailNotFoundException();

		productDetail.IsDeleted = true;
		await _productDetailRepository.UpdateAsync(productDetail);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> GetAllProductDetails()
    {
		var queryableProductDetails = (await _productDetailRepository.GetAllAsync())
			.Include(pd => pd.Product)
			.Include(pd => pd.ProductPrices)
			.Include(pd => pd.ProductAttributeValues)
			.ThenInclude(pav => pav.ProductAttribute);
		var productDetails = _mapper.Map<IEnumerable<DtoProductDetail>>(queryableProductDetails);

		return new ServiceActionResult(true) { Data = productDetails };
    }

    public async Task<ServiceActionResult> GetPaginatedProductDetails(BasePaginatedRequest request)
    {
		var queryableProductDetails = (await _productDetailRepository.GetAllAsync()).ProjectTo<DtoProductDetail>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(queryableProductDetails, request.PageSize, request.PageIndex);

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> GetProductDetailById(Guid id)
    {
		var productDetail = await (await _productDetailRepository.FindAsync(pd => !pd.IsDeleted && pd.Id == id))
			.ProjectTo<DtoSingleProductDetail>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync() ?? throw new ProductDetailNotFoundException();

		return new ServiceActionResult(true) { Data = productDetail };
    }

    public async Task<ServiceActionResult> UpdateProductDetail(UpdateProductDetailRequest request, Guid id)
    {
		if (!await _productDetailRepository.ExistsAsync(pd => pd.Id == id))
			throw new ProductDetailNotFoundException();

        throw new NotImplementedException();
    }

    public async Task<bool> IsAllProductDetailsExist(IEnumerable<Guid> requestProductDetailIds)
    {
	    foreach (var requestProductDetailId in requestProductDetailIds)
	    {
		    var productDetail =
			    await _productDetailRepository.GetAsync(x => x.Id == requestProductDetailId && x.IsDeleted == false) ??
			    throw new ProductDetailNotFoundException();
	    }

	    return true;
    }

    public async Task<IQueryable<ProductDetail>> GetProductDetails(List<Guid> productDetailsIds)
    {
	    // var tasks = productDetailsIds.Select(async productDetailsId =>
	    // {
		   //  var productDetail = await _productDetailRepository
			  //                       .GetAsync(x => x.Id == productDetailsId && x.IsDeleted == false) 
		   //                      ?? throw new ProductDetailNotFoundException();
		   //  return productDetail;
	    // });
	    // var productDetails = await Task.WhenAll(tasks);
	    // return productDetails.AsQueryable();
	    var productDetailList = new List<ProductDetail>();
	    foreach (var productDetailsId in productDetailsIds)
	    {
		    var productDetail = await _productDetailRepository
			                        .GetAsync((x => x.Id == productDetailsId && x.IsDeleted == false))
		                        ?? throw new ProductDetailNotFoundException();
		    productDetailList.Add(productDetail);
	    }
	    return productDetailList.AsQueryable();
    }

    public async Task<ServiceActionResult> AddImages(IEnumerable<IFormFile> images, Guid id)
    {
		var productDetail = await (await _productDetailRepository.FindAsync(pd => !pd.IsDeleted && pd.Id == id))
			.Include(pd => pd.ProductImages)
			.FirstOrDefaultAsync() ?? throw new ProductDetailNotFoundException();
		var uploadedImages = await _fileService.GetUrlAfterUploadedFile(images.ToList());

		foreach (var productImage in uploadedImages.Select(x => new ProductImage { ImageUrl = x }))
		{
			productDetail.ProductImages.Add(productImage);
		}

		await _productDetailRepository.UpdateAsync(productDetail);
		await _unitOfWork.CommitAsync();
		
		return new ServiceActionResult(true, "Images added successfully") { Data = uploadedImages };
    }

    public async Task<ServiceActionResult> SearchProductDetails(BaseSearchRequest request)
    {
	    var productDetails = (await _productDetailRepository.FindAsync(pk => !pk.IsDeleted)).ToList();
        
        foreach (var searchInfo in request.DisjunctionSearchInfos)
        {
	        productDetails = productDetails
                .Where(p => ReflectionHelper.GetStringValueByName(typeof(Package), searchInfo.FieldName, p)
                    .Contains(searchInfo.Keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (request.ConjunctionSearchInfos.Any())
        {
            var initialSearchInfo = request.ConjunctionSearchInfos.First();
            Expression<Func<ProductDetail, bool>> conjunctionWhere = p => ReflectionHelper.GetStringValueByName(typeof(ProductDetail), initialSearchInfo.FieldName, p)
                .Contains(initialSearchInfo.Keyword, StringComparison.OrdinalIgnoreCase);
            
            foreach (var (searchInfo, i) in request.ConjunctionSearchInfos.Select((value, i) => (value, i)))
            {
                if (i == 0)
                    continue;
                
                Expression<Func<ProductDetail, bool>> whereExpr = p => ReflectionHelper.GetStringValueByName(typeof(ProductDetail), searchInfo.FieldName, p)
                    .Contains(searchInfo.Keyword, StringComparison.OrdinalIgnoreCase);
                conjunctionWhere = ExpressionHelper.CombineOrExpressions(conjunctionWhere, whereExpr);
            }

            productDetails = productDetails.Where(conjunctionWhere.Compile()).ToList();
        }

        if (request.SortInfos.Any())
        {
            request.SortInfos = request.SortInfos.OrderBy(si => si.Priority).ToList();
            var initialSortInfo = request.SortInfos.First();
            Expression<Func<ProductDetail, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(ProductDetail), initialSortInfo.FieldName, p);

            productDetails = initialSortInfo.Descending ? productDetails.OrderByDescending(orderExpr.Compile()).ToList() : productDetails.OrderBy(orderExpr.Compile()).ToList();
            
            foreach (var (sortInfo, i) in request.SortInfos.Select((value, i) => (value, i)))
            {
                if (i == 0)
                    continue;
                
                orderExpr = p => ReflectionHelper.GetValueByName(typeof(Package), sortInfo.FieldName, p);
                productDetails = sortInfo.Descending ? productDetails.OrderByDescending(orderExpr.Compile()).ToList() : productDetails.OrderBy(orderExpr.Compile()).ToList();
            }
        }

        var paginatedResult = PaginationHelper.BuildPaginatedResult<ProductDetail, DtoProductDetail>(_mapper, productDetails, request.PageSize, request.PageIndex);

        return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> SearchProductDetailsUsingGet(SearchUsingGetRequest request)
    {
	    var productDetails = await (await _productDetailRepository.FindAsync(p => !p.IsDeleted))
		    .ProjectTo<DtoPackage>(_mapper.ConfigurationProvider)
		    .ToListAsync();
	    
	    if (!string.IsNullOrEmpty(request.SearchField))
	    {
		    productDetails = productDetails
			    .Where(p => ReflectionHelper.GetStringValueByName(typeof(DtoProductDetail), request.SearchField, p).Contains(request.SearchValue ?? string.Empty, StringComparison.OrdinalIgnoreCase))
			    .ToList();
	    }
	    if (!string.IsNullOrEmpty(request.SortField))
	    {
		    Expression<Func<DtoPackage, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(DtoProductDetail), request.SortField, p);
		    productDetails = request.Descending
			    ? productDetails.OrderByDescending(orderExpr.Compile()).ToList()
			    : productDetails.OrderBy(orderExpr.Compile()).ToList();
	    }

	    var paginatedResult = PaginationHelper.BuildPaginatedResult(productDetails, request.PageSize, request.PageIndex);
	    var finalProducts = (IEnumerable<DtoPackage>)paginatedResult.Items!;

	    paginatedResult.Items = finalProducts;

	    return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> CreateProductPrice(CreateProductPriceRequest request, Guid productDetailId)
    {
	    var productDetail = await (await _productDetailRepository.FindAsync(p => !p.IsDeleted && p.Id == productDetailId))
		    .Include(pd => pd.ProductPrices)
		    .FirstOrDefaultAsync() ?? throw new ProductDetailNotFoundException();

	    var newProductPrice = _mapper.Map<ProductPrice>(request);
	    productDetail.ProductPrices.Add(newProductPrice);
	    
	    await _productDetailRepository.UpdateAsync(productDetail);
	    await _unitOfWork.CommitAsync();
	    
	    return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> SearchProductDetailsInStorage(SearchUsingGetRequest request)
    {
	    var productsInStorage = (await _productDetailRepository.FindAsync(pd => !pd.IsDeleted))
		    .ProjectTo<DtoProductDetailInStorage>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(productsInStorage, request.PageSize, request.PageIndex);

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> ImportProductDetailsToStorage(IEnumerable<ImportProductDetailRequest> productDetails)
    {
		foreach (var productDetail in productDetails)
		{
			var retrievedDetail = await (await _productDetailRepository.FindAsync(p => !p.IsDeleted && p.Id == productDetail.ProductDetailId))
				.Include(pd => pd.ProductPrices)
				.FirstOrDefaultAsync() ?? throw new ProductDetailNotFoundException();

			var newProductPrice = new ProductPrice
			{
				Price = productDetail.Price,
				Quantity = productDetail.Quantity,
				MonetaryUnit = productDetail.MonetaryUnit,
				CreatedAt = DateTime.Now
			};

			retrievedDetail.ProductPrices.Add(newProductPrice);
			await _productDetailRepository.UpdateAsync(retrievedDetail);
		}
	    
	    await _unitOfWork.CommitAsync();
	    
	    return new ServiceActionResult(true);
    }
}
