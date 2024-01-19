using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;

[Route("api/[controller]")]
public class ProductsController : BaseApiController
{
	private IProductService _productService;

	public ProductsController(IProductService productService)
	{
		_productService = productService;
	}
	
	[HttpGet]
	public async Task<IActionResult> GetPaginatedProducts([FromQuery] BasePaginatedRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _productService.GetPaginatedProducts(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpGet("all")]
	public async Task<IActionResult> GetAllProducts()
	{
		return await ExecuteServiceLogic(
			async () => await _productService.GetAllProducts().ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetProduct(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _productService.GetProduct(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPost]
	public async Task<IActionResult> CreateProduct(CreateProductRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _productService.CreateProduct(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateProduct(UpdateProductRequest request, Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _productService.UpdateProduct(request, id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteProduct(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _productService.DeleteProduct(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
}
