using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Articles;
using Domus.Service.Models.Requests.Base;

namespace Domus.Service.Interfaces;

public interface IArticleService : IAutoRegisterable
{
	Task<ServiceActionResult> GetAllArticles();
	Task<ServiceActionResult> GetPaginatedArticles(BasePaginatedRequest request);
	Task<ServiceActionResult> CreateArticle(CreateArticleRequest request);
	Task<ServiceActionResult> UpdateArticle(UpdateArticleRequest request, Guid articleId);
	Task<ServiceActionResult> DeleteArticle(Guid articleId);
    Task<ServiceActionResult> GetArticle(Guid articleId);
}
