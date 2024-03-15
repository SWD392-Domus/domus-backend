using Domus.Api.Controllers.Base;
using Domus.Service.Constants;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Articles;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers; 

[Authorize(Roles = UserRoleConstants.INTERNAL_USER, AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
public class ArticlesController : BaseApiController
{
	private readonly IArticleService _articleService;

	public ArticlesController(IArticleService articleService)
	{
		_articleService = articleService;
	}

	[AllowAnonymous]
	[HttpGet]
	public async Task<IActionResult> GetPaginatedArticles([FromQuery] BasePaginatedRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _articleService.GetPaginatedArticles(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[AllowAnonymous]
	[HttpGet("all")]
	public async Task<IActionResult> GetAllArticles()
	{
		return await ExecuteServiceLogic(
			async () => await _articleService.GetAllArticles().ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[AllowAnonymous]
	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetArticle(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _articleService.GetArticle(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPost]
	public async Task<IActionResult> CreateArticle(CreateArticleRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _articleService.CreateArticle(request, GetJwtToken()).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateArticle(UpdateArticleRequest request, Guid articleId)
	{
		return await ExecuteServiceLogic(
			async () => await _articleService.UpdateArticle(request, articleId).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteArticle(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _articleService.DeleteArticle(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	[HttpGet("search")]
	[Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
	public async Task<IActionResult> SearchArticlesUsingGetRequest([FromQuery] SearchUsingGetRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _articleService.SearchArticlesUsingGet(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	[HttpDelete("many")]
	[Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
	public async Task<IActionResult> DeleteMultipleArticles(List<Guid> articleIds)
	{
		return await ExecuteServiceLogic(
			async () => await _articleService.DeleteArticles(articleIds).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	private string GetJwtToken()
	{
		var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
		return authorizationHeader.Remove(authorizationHeader.IndexOf("Bearer", StringComparison.Ordinal), "Bearer".Length).Trim();
	}
}
