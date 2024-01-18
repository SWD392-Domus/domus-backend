using Domus.Service.Models;

namespace Domus.Service.Interfaces;

public interface IArticleService
{
	Task<ServiceActionResult> GetAllArticles();
}
