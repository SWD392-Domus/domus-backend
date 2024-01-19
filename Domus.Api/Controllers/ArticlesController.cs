using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Articles;
using Domus.Service.Models.Requests.Base;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;

[Route("api/[controller]")]
public class ArticlesController : BaseApiController
{
	private readonly IArticleService _articleService;

	public ArticlesController(IArticleService articleService)
	{
		_articleService = articleService;
	}

	[HttpGet]
	public async Task<IActionResult> GetPaginatedArticles([FromQuery] BasePaginatedRequest request)
	{
		return await ExecuteServiceLogic(
				async () => await _articleService.GetPaginatedArticles(request).ConfigureAwait(false)
				).ConfigureAwait(false);
	}

	[HttpGet("all")]
	public async Task<IActionResult> GetAllArticles()
	{
		return await ExecuteServiceLogic(
				async () => await _articleService.GetAllArticles().ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPost("create")]
	public async Task<IActionResult> CreateArticle(CreateArticleRequest request)
	{
		return await ExecuteServiceLogic(
				async () => await _articleService.CreateArticle(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPut("update")]
	public async Task<IActionResult> UpdateArticle(UpdateArticleRequest request)
	{
		return await ExecuteServiceLogic(
				async () => await _articleService.UpdateArticle(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpDelete("delete")]
	public async Task<IActionResult> DeleteArticle(DeleteArticleRequest request)
	{
		return await ExecuteServiceLogic(
				async () => await _articleService.DeleteArticle(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
}
