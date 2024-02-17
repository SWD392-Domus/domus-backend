using System.Linq.Expressions;
using System.Reflection;
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
			IProductCategoryRepository productCategoryRepository)
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

		return new ServiceActionResult(true);
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
			.ProjectTo<DtoProductWithoutCategory>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync() ?? throw new ProductNotFoundException();

		return new ServiceActionResult(true) { Data = product };
    }

    public async Task<ServiceActionResult> UpdateProduct(UpdateProductRequest request, Guid id)
    {
		var product = await _productRepository.GetAsync(p => !p.IsDeleted && p.Id == id);
		if (product is null)
			throw new ProductNotFoundException();
		if (!await _productCategoryRepository.ExistsAsync(c => c.Id == request.ProductCategoryId))
			throw new ProductCategoryNotFoundException();

		_mapper.Map(product, request);
		await _productRepository.UpdateAsync(product);
		await _unitOfWork.CommitAsync();
		
		return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> DeleteMultipleProducts(IEnumerable<Guid> ids)
    {
	    await _productRepository.DeleteManyAsync(p => !p.IsDeleted && ids.Contains(p.Id));
	    await _unitOfWork.CommitAsync();
	    
	    return new ServiceActionResult(true) { Detail = "Products deleted successfully"};
    }

    public async Task<ServiceActionResult> SearchProducts(BaseSearchRequest request)
    {
	    var products = await (await _productRepository.GetAllAsync()).ToListAsync();
	    
	    foreach (var searchInfo in request.DisjunctionSearchInfos)
	    {
			products = products
				.Where(p => ReflectionHelper.GetStringValueByName(typeof(Product), searchInfo.FieldName, p).Contains(searchInfo.Keyword, StringComparison.OrdinalIgnoreCase))
				.ToList();
	    }

	    if (request.ConjunctionSearchInfos.Any())
	    {
			var initialSearchInfo = request.ConjunctionSearchInfos.First();
			Expression<Func<Product, bool>> conjunctionWhere = p => ReflectionHelper.GetStringValueByName(typeof(Product), initialSearchInfo.FieldName, p).Contains(initialSearchInfo.Keyword, StringComparison.OrdinalIgnoreCase);
			
			foreach (var (searchInfo, i) in request.ConjunctionSearchInfos.Select((value, i) => (value, i)))
			{
				if (i == 0)
					continue;
				
				Expression<Func<Product, bool>> whereExpr =  p => ReflectionHelper.GetStringValueByName(typeof(Product), searchInfo.FieldName, p).Contains(searchInfo.Keyword, StringComparison.OrdinalIgnoreCase);
				conjunctionWhere = ExpressionHelper.CombineOrExpressions(conjunctionWhere, whereExpr);
			}

			products = products.Where(conjunctionWhere.Compile()).ToList();
	    }

	    if (request.SortInfos.Any())
	    {
			request.SortInfos = request.SortInfos.OrderBy(si => si.Priority).ToList();
			var initialSortInfo = request.SortInfos.First();
			Expression<Func<Product, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(Product), initialSortInfo.FieldName, p);

			products = initialSortInfo.Descending ? products.OrderByDescending(orderExpr.Compile()).ToList() : products.OrderBy(orderExpr.Compile()).ToList();
			
			foreach (var (sortInfo, i) in request.SortInfos.Select((value, i) => (value, i)))
			{
				if (i == 0)
					continue;
				
				orderExpr = p => ReflectionHelper.GetValueByName(typeof(Product), initialSortInfo.FieldName, p);
				products = sortInfo.Descending ? products.OrderByDescending(orderExpr.Compile()).ToList() : products.OrderBy(orderExpr.Compile()).ToList();
			}
	    }

	    var paginatedResult = PaginationHelper.BuildPaginatedResult<Product, DtoProduct>(_mapper, products, request.PageSize, request.PageIndex);

	    return new ServiceActionResult(true) { Data = paginatedResult };
    }
}