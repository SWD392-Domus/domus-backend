using Domus.Api.Controllers.Base;
using Domus.Service.Constants;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;
using Domus.Service.Models.Requests.Quotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
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
			async () => await _quotationService.GetPaginatedQuotations(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpGet("all")]
	public async Task<IActionResult> GetAllQuotations()
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.GetAllQuotations().ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetQuotationById(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.GetQuotationById(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpGet("search")]
	public async Task<IActionResult> SearchQuotations([FromQuery] SearchUsingGetRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.SearchQuotations(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPost]
	public async Task<IActionResult> CreateQuotation(CreateQuotationRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.CreateQuotation(request, GetJwtToken()).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateQuotation(UpdateQuotationRequest request, Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.UpdateQuotation(request, id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteQuotation(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.DeleteQuotation(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
	[HttpDelete("multiple")]
	public async Task<IActionResult> DeleteMultipleQuotations(IEnumerable<Guid> ids)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.DeleteMultipleQuotations(ids).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPost("{id:guid}/negotiations/messages")]
	public async Task<IActionResult> CreateNegotiationMessage(CreateNegotiationMessageRequest request, Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.CreateNegotiationMessage(request, id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpGet("{id:guid}/negotiations/messages")]
	public async Task<IActionResult> GetPaginatedNegotiationMessages([FromQuery] BasePaginatedRequest request, Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.GetPaginatedNegotiationMessages(request, id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpGet("{id:guid}/negotiations/messages/all")]
	public async Task<IActionResult> GetAllNegotiationMessages(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.GetAllNegotiationMessages(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	private string GetJwtToken()
	{
		var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
		return authorizationHeader.Remove(authorizationHeader.IndexOf("Bearer", StringComparison.Ordinal), "Bearer".Length).Trim();
	}

	[HttpGet("my-quotation")]
	[Authorize(Roles = UserRoleConstants.CLIENT)]
	public async Task<IActionResult> GetUserQuotationHistory()
	{
		return await ExecuteServiceLogic(async () => await _quotationService.GetUserQuotationHistory(GetJwtToken())).ConfigureAwait(false);
	}
	
	[HttpGet("/api/customer/quotations/search")]
	public async Task<IActionResult> GetMyQuotations([FromQuery] SearchUsingGetRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.SearchUserQuotations(request, GetJwtToken()).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	[HttpGet("{id:guid}/negotiations")]
	public async Task<IActionResult> GetQuotationPriceChangeHistory(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.GetQuotationPriceChangeHistory(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	[HttpGet("{id:guid}/revisions")]
	public async Task<IActionResult> GetQuotationRevisions(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.GetQuotationRevisions(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	[HttpGet("{quotationId:guid}/revisions/{revisionId:guid}")]
	public async Task<IActionResult> GetQuotationRevision(Guid quotationId, Guid revisionId)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.GetQuotationRevision(quotationId, revisionId).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	[HttpPut("{quotationId:guid}/status")]
	public async Task<IActionResult> UpdateQuotationStatus(Guid quotationId, [FromBody] UpdateQuotationStatusRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.UpdateQuotationStatus(quotationId, request.Status).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[Authorize(Roles = UserRoleConstants.STAFF)]
	[HttpGet("/api/staff/quotations/search")]
	public async Task<IActionResult> GetStaffQuotations([FromQuery] SearchUsingGetRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _quotationService.SearchStaffQuotations(request, GetJwtToken()).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
}
