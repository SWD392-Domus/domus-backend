using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
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

	[HttpGet("all")]
	public async Task<IActionResult> GetAllArticles()
	{
		return await ExecuteServiceLogic(
				async () => await _articleService.GetAllArticles().ConfigureAwait(false)
		).ConfigureAwait(false);
	}
}
