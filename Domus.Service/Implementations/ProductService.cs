using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;

namespace Domus.Service.Implementations;

public class ProductService : IProductService
{
	private readonly IProductRepository _productRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	
	public ProductService(
			IProductRepository productRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper)
	{
		_productRepository = productRepository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

    public Task<ServiceActionResult> CreateProduct(CreateProductRequest request)
    {
        throw new NotImplementedException();
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
		var products = await _productRepository.GetAllAsync();

		return new ServiceActionResult(true) { Data = _mapper.Map<IEnumerable<DtoProduct>>(products) };
    }

    public async Task<ServiceActionResult> GetPaginatedProducts(BasePaginatedRequest request)
    {
		var queryableProducts = (await _productRepository.GetAllAsync()).ProjectTo<DtoProduct>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(queryableProducts, request.PageSize, request.PageIndex);

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> GetProduct(Guid id)
    {
		var product = await _productRepository.GetAsync(p => p.Id == id);
		if (product is null)
			throw new ProductNotFoundException();

		return new ServiceActionResult(true) { Data = _mapper.Map<DtoProduct>(product) };
    }

    public Task<ServiceActionResult> UpdateProduct(UpdateProductRequest request, Guid id)
    {
        throw new NotImplementedException();
    }
}
