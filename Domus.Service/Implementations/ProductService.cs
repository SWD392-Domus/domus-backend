using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Exceptions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos.Products;
using Domus.Domain.Entities;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;
using Microsoft.EntityFrameworkCore;

namespace Domus.Service.Implementations;

public class ProductService : IProductService
{
	private readonly IProductRepository _productRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IProductCategoryRepository _productCategoryRepository;
	
	public ProductService(
			IProductRepository productRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			IProductCategoryRepository productCategoryRepository, IProductDetailRepository productDetailRepository)
	{
		_productRepository = productRepository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_productCategoryRepository = productCategoryRepository;
	}

    public async Task<ServiceActionResult> CreateProduct(CreateProductRequest request)
    {
		if (!await _productCategoryRepository.ExistsAsync(c => c.Id == request.ProductCategoryId))
			throw new ProductCategoryNotFoundException();

		var product = _mapper.Map<Product>(request);
		await _productRepository.AddAsync(product);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true) { Data = _mapper.Map<DtoProduct>(product) };
    }

    public async Task<ServiceActionResult> DeleteProduct(Guid id)
    {
		var product = await _productRepository.GetAsync(p => !p.IsDeleted && p.Id == id);
		if (product is null)
			throw new ProductNotFoundException();

		product.IsDeleted = true;
		await _productRepository.UpdateAsync(product);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> GetAllProducts()
    {
		var products = await (await _productRepository.GetAllAsync())
			.Where(p => !p.IsDeleted)
			.ProjectTo<DtoProduct>(_mapper.ConfigurationProvider)
			.ToListAsync();

		foreach (var product in products)
		{
			product.TotalQuantity = (int)product.ProductDetails.Sum(d => d.ProductPrices.Sum(p => p.Quantity));
		}
		
		return new ServiceActionResult(true) { Data = products };
    }

    public async Task<ServiceActionResult> GetPaginatedProducts(BasePaginatedRequest request)
    {
		var queryableProducts = (await _productRepository.GetAllAsync())
			.Where(p => !p.IsDeleted)
			.ProjectTo<DtoProduct>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(queryableProducts, request.PageSize, request.PageIndex);
		var products = await ((IQueryable<DtoProduct>)paginatedResult.Items!).ToListAsync();

		foreach (var product in products)
		{
			product.TotalQuantity = (int)product.ProductDetails.Sum(d => d.ProductPrices.Sum(p => p.Quantity));
		}

		paginatedResult.Items = products;

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> GetProduct(Guid id)
    {
		var product = await (await _productRepository.GetAllAsync())
			.Where(p => !p.IsDeleted && p.Id == id)
			.ProjectTo<DtoProduct>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync() ?? throw new ProductNotFoundException();

		return new ServiceActionResult(true) { Data = product };
    }

    public async Task<ServiceActionResult> UpdateProduct(UpdateProductRequest request, Guid id)
    {
		var product = await (await _productRepository.FindAsync(p => !p.IsDeleted && p.Id == id))
			.Include(p => p.ProductDetails)
			.ThenInclude(pd => pd.ProductPrices)
			.Include(p => p.ProductDetails)
			.ThenInclude(pd => pd.ProductImages)
			.Include(p => p.ProductDetails)
			.ThenInclude(pd => pd.ProductAttributeValues)
			.FirstOrDefaultAsync() ?? throw new ProductNotFoundException();
		if (request.ProductCategoryId != default && !await _productCategoryRepository.ExistsAsync(c => !c.IsDeleted && c.Id == request.ProductCategoryId))
			throw new ProductCategoryNotFoundException();

		_mapper.Map(request, product);

		foreach (var productDetail in request.ProductDetails)
		{
			if (productDetail.Id == default)
			{
				var newProductDetail = _mapper.Map<ProductDetail>(productDetail);
				product.ProductDetails.Add(newProductDetail);
			}
			else
			{
				if (!product.ProductDetails.Any(pd => !pd.IsDeleted && pd.Id == productDetail.Id)) continue;
				
				var detail = product.ProductDetails.FirstOrDefault(pd => !pd.IsDeleted && pd.Id == productDetail.Id);

				if (detail == null) continue;
				
				product.ProductDetails.Remove(detail);
				foreach (var attributeValue in productDetail.ProductAttributeValues)
				{
					if (attributeValue.AttributeId == default)
					{
						detail.ProductAttributeValues.Add(_mapper.Map<ProductAttributeValue>(attributeValue));
					}
					else
					{
						var attr = detail.ProductAttributeValues.FirstOrDefault(pav =>
							pav.ProductAttributeId == attributeValue.AttributeId);
						
						if (attr == null) continue;

						var attributeValueList = new List<ProductAttributeValue>(detail.ProductAttributeValues.Where(pav =>
							pav.ProductAttributeId == attributeValue.AttributeId));
						foreach (var productAttributeValue in attributeValueList)
						{
							detail.ProductAttributeValues.Remove(productAttributeValue);
						}
						var updatedAttr = _mapper.Map<ProductAttributeValue>(attributeValue);
						detail.ProductAttributeValues.Add(updatedAttr);
					}
				}

				var detailAttributeValueList = new List<ProductAttributeValue>(detail.ProductAttributeValues);
				foreach (var attributeValue in detailAttributeValueList)
				{
					if (attributeValue.Id != default && !productDetail.ProductAttributeValues.Select(pd => pd.AttributeId).Contains(attributeValue.Id))
						detail.ProductAttributeValues.Remove(attributeValue);
				}
				
				foreach (var productPrice in productDetail.ProductPrices)
				{
					if (productPrice.Id == default)
					{
						detail.ProductPrices.Add(_mapper.Map<ProductPrice>(productPrice));
					}
					else
					{
						var price = detail.ProductPrices.FirstOrDefault(p =>
							p.Id == productPrice.Id);
						
						if (price == null) continue;
						
						detail.ProductPrices.Remove(price);
						var updatedPrice = _mapper.Map<ProductPrice>(productPrice);
						detail.ProductPrices.Add(updatedPrice);
					}
				}

				var productPriceList = new List<ProductPrice>(detail.ProductPrices);
				foreach (var productPrice in productPriceList)
				{
					if (productPrice.Id != default && productPriceList.Select(p => p.Id).Contains(productPrice.Id))
						detail.ProductPrices.Remove(productPrice);
				}
				
				product.ProductDetails.Add(detail);
			}
		}

		foreach (var productDetail in product.ProductDetails)
		{
			if (!request.ProductDetails.Select(pd => pd.Id).Contains(productDetail.Id))
				productDetail.IsDeleted = true;
		}

		await _productRepository.UpdateAsync(product);
		await _unitOfWork.CommitAsync();
		
		var updatedProduct = await (await _productRepository.FindAsync(p => !p.IsDeleted && p.Id == id))
			.ProjectTo<DtoProduct>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync() ?? throw new ProductNotFoundException();
		
		return new ServiceActionResult(true) { Data = updatedProduct };
    }

    public async Task<ServiceActionResult> DeleteMultipleProducts(IEnumerable<Guid> ids)
    {
	    await _productRepository.DeleteManyAsync(p => !p.IsDeleted && ids.Contains(p.Id));
	    await _unitOfWork.CommitAsync();
	    
	    return new ServiceActionResult(true) { Detail = "Products deleted successfully"};
    }

    public async Task<ServiceActionResult> SearchProducts(BaseSearchRequest request)
    {
	    var products = await (await _productRepository.FindAsync(p => !p.IsDeleted))
		    .ProjectTo<DtoProduct>(_mapper.ConfigurationProvider)
		    .ToListAsync();
	    
	    foreach (var searchInfo in request.DisjunctionSearchInfos)
	    {
			products = products
				.Where(p => ReflectionHelper.GetStringValueByName(typeof(DtoProduct), searchInfo.FieldName, p).Contains(searchInfo.Keyword, StringComparison.OrdinalIgnoreCase))
				.ToList();
	    }

	    if (request.ConjunctionSearchInfos.Any())
	    {
			var initialSearchInfo = request.ConjunctionSearchInfos.First();
			Expression<Func<DtoProduct, bool>> conjunctionWhere = p => ReflectionHelper.GetStringValueByName(typeof(DtoProduct), initialSearchInfo.FieldName, p).Contains(initialSearchInfo.Keyword, StringComparison.OrdinalIgnoreCase);
			
			foreach (var (searchInfo, i) in request.ConjunctionSearchInfos.Select((value, i) => (value, i)))
			{
				if (i == 0)
					continue;
				
				Expression<Func<DtoProduct, bool>> whereExpr =  p => ReflectionHelper.GetStringValueByName(typeof(Product), searchInfo.FieldName, p).Contains(searchInfo.Keyword, StringComparison.OrdinalIgnoreCase);
				conjunctionWhere = ExpressionHelper.CombineOrExpressions(conjunctionWhere, whereExpr);
			}

			products = products.Where(conjunctionWhere.Compile()).ToList();
	    }

	    if (request.SortInfos.Any())
	    {
			request.SortInfos = request.SortInfos.OrderBy(si => si.Priority).ToList();
			var initialSortInfo = request.SortInfos.First();
			Expression<Func<DtoProduct, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(DtoProduct), initialSortInfo.FieldName, p);

			
			var orderedProducts = initialSortInfo.Descending ? products.OrderByDescending(orderExpr.Compile()) : products.OrderBy(orderExpr.Compile());
			
			foreach (var (sortInfo, i) in request.SortInfos.Select((value, i) => (value, i)))
			{
				if (i == 0)
					continue;
				
				orderExpr = p => ReflectionHelper.GetValueByName(typeof(DtoProduct), initialSortInfo.FieldName, p);
				orderedProducts = sortInfo.Descending ? orderedProducts.ThenByDescending(orderExpr.Compile()) : orderedProducts.ThenBy(orderExpr.Compile());
			}

			products = orderedProducts.ToList();
	    }

	    var paginatedResult = PaginationHelper.BuildPaginatedResult(products, request.PageSize, request.PageIndex);
	    var finalProducts = (IEnumerable<DtoProduct>)paginatedResult.Items!;

	    foreach (var product in finalProducts)
	    {
		    product.TotalQuantity = (int)product.ProductDetails.Sum(d => d.ProductPrices.Sum(p => p.Quantity));
	    }

	    paginatedResult.Items = finalProducts;

	    return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> SearchProductsUsingGet(SearchUsingGetRequest request)
    {
		var products = await (await _productRepository.FindAsync(p => !p.IsDeleted))
		    .ProjectTo<DtoProduct>(_mapper.ConfigurationProvider)
		    .ToListAsync();
	    
	    if (!string.IsNullOrEmpty(request.SearchField))
	    {
			products = products
				.Where(p => ReflectionHelper.GetStringValueByName(typeof(DtoProduct), request.SearchField, p).Contains(request.SearchValue ?? string.Empty, StringComparison.OrdinalIgnoreCase))
				.ToList();
	    }
	    
	    foreach (var product in products)
	    {
		    product.TotalQuantity = (int)product.ProductDetails.Sum(d => d.ProductPrices.Sum(p => p.Quantity));
	    }

	    if (!string.IsNullOrEmpty(request.SortField))
	    {
			Expression<Func<DtoProduct, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(DtoProduct), request.SortField, p);
			products = request.Descending
				? products.OrderByDescending(orderExpr.Compile()).ToList()
				: products.OrderBy(orderExpr.Compile()).ToList();
	    }

	    var paginatedResult = PaginationHelper.BuildPaginatedResult(products, request.PageSize, request.PageIndex);
	    var finalProducts = (IEnumerable<DtoProduct>)paginatedResult.Items!;

	    paginatedResult.Items = finalProducts;

	    return new ServiceActionResult(true) { Data = paginatedResult };
    }
}
