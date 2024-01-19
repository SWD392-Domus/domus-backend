using Domus.Service.Models;
using Domus.Service.Models.Requests.Articles;
using Domus.Service.Models.Requests.Base;

namespace Domus.Service.Interfaces;

public interface IArticleService
{
	Task<ServiceActionResult> GetAllArticles();
	Task<ServiceActionResult> GetPaginatedArticles(BasePaginatedRequest request);
	Task<ServiceActionResult> CreateArticle(CreateArticleRequest request);
	Task<ServiceActionResult> UpdateArticle(UpdateArticleRequest request);
	Task<ServiceActionResult> DeleteArticle(DeleteArticleRequest request);
}
