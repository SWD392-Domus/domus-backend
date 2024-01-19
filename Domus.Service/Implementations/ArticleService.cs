using AutoMapper;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Articles;
using Domus.Service.Models.Requests.Base;
using NetCore.WebApiCommon.Core.Common.Helpers;

namespace Domus.Service.Implementations;

public class ArticleService : IArticleService
{
	private readonly IArticleRepository _articleRepository;
	private readonly IArticleCategoryRepository _articleCategoryRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public ArticleService(
			IArticleRepository articleRepository,
			IArticleCategoryRepository articleCategoryRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper)
	{
		_articleRepository = articleRepository;
		_articleCategoryRepository = articleCategoryRepository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

    public async Task<ServiceActionResult> CreateArticle(CreateArticleRequest request)
    {
		var article = _mapper.Map<Article>(request);
		var articleCategory = await _articleCategoryRepository.GetAsync(c => c.Id == request.CategoryId);
		if (articleCategory is null)
			throw new ArticleCategoryNotFoundException();

		await _articleRepository.AddAsync(article);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    public Task<ServiceActionResult> DeleteArticle(DeleteArticleRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> GetAllArticles()
    {
		var articles = await _articleRepository.GetAllAsync();

		return new ServiceActionResult(true) { Data = articles };
    }

    public async Task<ServiceActionResult> GetPaginatedArticles(BasePaginatedRequest request)
    {
		var paginatedResult = PaginationHelper.BuildPaginatedResult(await _articleRepository.GetAllAsync(), request.PageSize, request.PageIndex);

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public Task<ServiceActionResult> UpdateArticle(UpdateArticleRequest request)
    {
        throw new NotImplementedException();
    }
}
