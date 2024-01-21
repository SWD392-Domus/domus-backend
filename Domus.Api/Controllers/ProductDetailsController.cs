using Domus.Api.Controllers.Base;
using Domus.Service.Constants;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests;
using Domus.Service.Models.Requests.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;

[Authorize(Roles = $"{UserRoleConstants.ADMIN},{UserRoleConstants.STAFF}", AuthenticationSchemes = "Bearer")]
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
}
