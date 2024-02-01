using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Quotations;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;

[Route("api/[controller]")]
public class QuotationsController : BaseApiController
{
	private readonly IQuotationService _quotationService;

	public QuotationsController(IQuotationService quotationService)
	{
		_quotationService = quotationService;
	}

	[HttpGet]
	public async Task<IActionResult> GetPaginatedQuotations([FromQuery] BasePaginatedRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.GetPaginatedQuotations(request)
		);
	}

	[HttpGet("all")]
	public async Task<IActionResult> GetAllQuotations()
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.GetAllQuotations()
		);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetQuotationById(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.GetQuotationById(id)
		);
	}

	[HttpPost]
	public async Task<IActionResult> CreateQuotation(CreateQuotationRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.CreateQuotation(request)
		);
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateQuotation(UpdateQuotationRequest request, Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.UpdateQuotation(request, id)
		);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteQuotation(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.DeleteQuotation(id)
		);
	}

	[HttpPost("{id:guid}/negotations/messages")]
	public async Task<IActionResult> CreateNegotiationMessage(CreateNegotiationMessageRequest request, Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.CreateNegotiationMessage(request, id)
		);
	}
}
