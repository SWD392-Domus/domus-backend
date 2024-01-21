using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests;
using Domus.Service.Models.Requests.Base;
using Microsoft.EntityFrameworkCore;

namespace Domus.Service.Implementations;

public class ProductDetailService : IProductDetailService
{
	private readonly IProductDetailRepository _productDetailRepository;
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public ProductDetailService(
			IProductDetailRepository productDetailRepository,
			IMapper mapper,
			IUnitOfWork unitOfWork,
			IProductRepository productRepository)
	{
		_productDetailRepository = productDetailRepository;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_productRepository = productRepository;
	}

    public async Task<ServiceActionResult> CreateProductDetail(CreateProductDetailRequest request)
    {
		if (!await _productRepository.ExistsAsync(p => p.Id == request.ProductId))
			throw new ProductNotFoundException();

		var productDetail = _mapper.Map<ProductDetail>(request);
		await _productDetailRepository.AddAsync(productDetail);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true) { Data = _mapper.Map<DtoProductDetail>(productDetail) };
    }

    public async Task<ServiceActionResult> DeleteProductDetail(Guid id)
    {
		if (!await _productDetailRepository.ExistsAsync(pd => pd.Id == id))
			throw new ProductDetailNotFoundException();

		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> GetAllProductDetails()
    {
		var queryableProductDetails = (await _productDetailRepository.GetAllAsync())
			.Include(pd => pd.ProductPrices);
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
}
