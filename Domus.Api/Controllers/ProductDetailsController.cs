using Domus.Api.Controllers.Base;
using Domus.Service.Constants;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.ProductDetails;
using Domus.Service.Models.Requests.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;

[Authorize(Roles = UserRoleConstants.INTERNAL_USER, AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
public class ProductDetailsController : BaseApiController
{
	private readonly IProductDetailService _productDetailService;

	public ProductDetailsController(IProductDetailService productDetailService)
	{
		_productDetailService = productDetailService;
	}

	[AllowAnonymous]
	[HttpGet]
	public async Task<IActionResult> GetPaginatedProductDetails([FromQuery] BasePaginatedRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _productDetailService.GetPaginatedProductDetails(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[AllowAnonymous]
	[HttpGet("all")]
	public async Task<IActionResult> GetAllProductDetails()
	{
		return await ExecuteServiceLogic(
			async () => await _productDetailService.GetAllProductDetails().ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[AllowAnonymous]
	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetProductDetail(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _productDetailService.GetProductDetailById(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPost]
	public async Task<IActionResult> CreateProductDetail(CreateProductDetailRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _productDetailService.CreateProductDetail(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	[HttpPost("{id:guid}/images")]
	public async Task<IActionResult> AddImagesToProductDetail(IEnumerable<IFormFile> images, Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _productDetailService.AddImages(images, id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}


	[HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateProductDetail(UpdateProductDetailRequest request, Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _productDetailService.UpdateProductDetail(request, id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteProductDetail(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _productDetailService.DeleteProductDetail(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	[HttpPost("search")]
	public async Task<IActionResult> SearchProductDetails(BaseSearchRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _productDetailService.SearchProductDetails(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	[HttpGet("search")]
	public async Task<IActionResult>  SearchProductDetailsGetRequest([FromQuery] SearchUsingGetRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _productDetailService.SearchProductDetailsUsingGet(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
}
