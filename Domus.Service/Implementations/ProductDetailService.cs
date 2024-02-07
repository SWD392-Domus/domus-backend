using System.Collections;
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
using Domus.Service.Models.Requests.ProductDetails;
using Microsoft.EntityFrameworkCore;
namespace Domus.Service.Implementations;

public class ProductDetailService : IProductDetailService
{
	private readonly IProductDetailRepository _productDetailRepository;
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IProductPriceRepository _productPriceRepository;
	private readonly IProductAttributeValueRepository _productAttributeValueRepository;
	private readonly IProductAttributeRepository _productAttributeRepository;

	public ProductDetailService(
			IProductDetailRepository productDetailRepository,
			IMapper mapper,
			IUnitOfWork unitOfWork,
			IProductRepository productRepository,
			IProductPriceRepository productPriceRepository,
			IProductAttributeValueRepository productAttributeValueRepository,
			IProductAttributeRepository productAttributeRepository)
	{
		_productDetailRepository = productDetailRepository;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_productRepository = productRepository;
		_productPriceRepository = productPriceRepository;
		_productAttributeValueRepository = productAttributeValueRepository;
		_productAttributeRepository = productAttributeRepository;
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
		var productDetail = await _productDetailRepository.GetAsync(pd => pd.Id == id);
		if (productDetail == null)
			throw new ProductDetailNotFoundException();

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
		var productDetail = await _productDetailRepository.GetAsync(pd => pd.Id == id);
		if (productDetail == null)
			throw new ProductDetailNotFoundException();

		return new ServiceActionResult(true) { Data = _mapper.Map<DtoProductDetail>(productDetail) };
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
}
