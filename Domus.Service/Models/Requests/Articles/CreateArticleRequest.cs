using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Articles;

public class CreateArticleRequest
{
	[Required]
	public Guid ArticleCategoryId { get; set; }

	[Required]
	public string Title { get; set; } = null!;

	[Required]
	public string Content { get; set; } = null!;

	// public ICollection<ArticleImageInCreateArticleRequest> Type { get; set; }
}
