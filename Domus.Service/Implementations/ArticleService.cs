using AutoMapper;
using Domus.DAL.Interfaces;
using Domus.Service.Interfaces;
using Domus.Service.Models;

namespace Domus.Service.Implementations;

public class ArticleService : IArticleService
{
	private readonly IArticleRepository _articleRepository;
	private readonly IMapper _mapper;

	public ArticleService(
			IArticleRepository articleRepository,
			IMapper mapper)
	{
		_articleRepository = articleRepository;
		_mapper = mapper;
	}

    public async Task<ServiceActionResult> GetAllArticles()
    {
		var articles = await _articleRepository.GetAllAsync();

		return new ServiceActionResult(true) { Data = articles };
    }
}
