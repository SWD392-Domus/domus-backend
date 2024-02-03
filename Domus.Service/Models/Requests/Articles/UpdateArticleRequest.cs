using Domus.Domain.Dtos;

namespace Domus.Service.Models.Requests.Articles;

public class UpdateArticleRequest
{
	public Guid ArticleCategoryId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public virtual ICollection<DtoArticleImage> ArticleImages { get; set; } = new List<DtoArticleImage>();

	public bool ReplaceImages { get; set; }
}
