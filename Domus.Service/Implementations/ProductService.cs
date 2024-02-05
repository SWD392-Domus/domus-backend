using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Exceptions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
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
		var product = await _productRepository.GetAsync(p => p.Id == id);
		if (product is null)
			throw new ProductNotFoundException();

		product.IsDeleted = true;
		await _productRepository.UpdateAsync(product);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> GetAllProducts()
    {
		var products = await (await _productRepository.GetAllAsync()).ProjectTo<DtoProductWithoutCategory>(_mapper.ConfigurationProvider)
			.ToListAsync();
		
		return new ServiceActionResult(true) { Data = products };
    }

    public async Task<ServiceActionResult> GetPaginatedProducts(BasePaginatedRequest request)
    {
		var queryableProducts = (await _productRepository.GetAllAsync()).ProjectTo<DtoProductWithoutCategory>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(queryableProducts, request.PageSize, request.PageIndex);

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> GetProduct(Guid id)
    {
		var product = await (await _productRepository.GetAllAsync())
			.Where(p => p.Id == id)
			.Include(p => p.ProductCategory)
			.Include(p => p.ProductDetails)
			.FirstOrDefaultAsync() ?? throw new ProductNotFoundException();

		return new ServiceActionResult(true) { Data = _mapper.Map<DtoProduct>(product) };
    }

    public async Task<ServiceActionResult> UpdateProduct(UpdateProductRequest request, Guid id)
    {
		var product = await _productRepository.GetAsync(p => p.Id == id);
		if (product is null)
			throw new ProductNotFoundException();
		if (!await _productCategoryRepository.ExistsAsync(c => c.Id == request.ProductCategoryId))
			throw new ProductCategoryNotFoundException();

		_mapper.Map(product, request);
		await _productRepository.UpdateAsync(product);
		await _unitOfWork.CommitAsync();
		
		return new ServiceActionResult(true);
    }
}
