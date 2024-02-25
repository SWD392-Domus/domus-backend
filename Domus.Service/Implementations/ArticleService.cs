using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos.Articles;
using Domus.Domain.Entities;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Articles;
using Domus.Service.Models.Requests.Base;
using Microsoft.EntityFrameworkCore;

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
		if (!await _articleCategoryRepository.ExistsAsync(c => c.Id == request.ArticleCategoryId))
			throw new ArticleCategoryNotFoundException();

		var article = _mapper.Map<Article>(request);
		article.CreatedBy = "1729f5b6-3bee-48f3-bc29-f59bffb084d2";
		article.CreatedAt = DateTime.Now;
		article.LastUpdatedAt = DateTime.Now;
		await _articleRepository.AddAsync(article);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> DeleteArticle(Guid articleId)
    {
		var article = await _articleRepository.GetAsync(a => a.Id == articleId);
		if (article is null)
			throw new ArticleNotFoundException();

		article.IsDeleted = true;
		article.LastUpdatedAt = DateTime.Now;
		await _articleRepository.UpdateAsync(article);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> GetAllArticles()
    {
		var articles = await _articleRepository.GetAllAsync();

		return new ServiceActionResult(true) { Data = _mapper.Map<IEnumerable<DtoArticleWithoutCategory>>(articles) };
    }

    public async Task<ServiceActionResult> GetArticle(Guid articleId)
    {
		var article = await _articleRepository.GetAsync(a => a.Id == articleId);
		if (article is null)
			throw new ArticleNotFoundException();

		return new ServiceActionResult(true) { Data = _mapper.Map<DtoArticleWithoutCategory>(article) };
    }

    public async Task<ServiceActionResult> GetPaginatedArticles(BasePaginatedRequest request)
    {
		var queryableArticles = (await _articleRepository.GetAllAsync()).ProjectTo<DtoArticle>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(queryableArticles, request.PageSize, request.PageIndex);

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> UpdateArticle(UpdateArticleRequest request, Guid articleId)
    {
		if (!await _articleCategoryRepository.ExistsAsync(c => c.Id == request.ArticleCategoryId))
			throw new ArticleCategoryNotFoundException();

		var article = await (await _articleRepository.FindAsync(a => a.Id == articleId))
			.Include(a => a.ArticleImages)
			.FirstOrDefaultAsync(); 
		if (article is null)
			throw new ArticleNotFoundException();

		_mapper.Map(article, request);
		if (request.ReplaceImages)
			article.ArticleImages = _mapper.Map<ICollection<ArticleImage>>(request.ArticleImages);
		else 
		{
			foreach (var image in _mapper.Map<ICollection<ArticleImage>>(request.ArticleImages))
			{
				article.ArticleImages.Add(image);
			}
		}

		await _articleRepository.UpdateAsync(article);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }
}
