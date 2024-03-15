using System.Text.Json.Serialization;
using Domus.Domain.Dtos.Articles;

namespace Domus.Service.Models.Requests.Articles;

public class UpdateArticleRequest
{
	public Guid? ArticleCategoryId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

	[JsonIgnore]
    public virtual ICollection<DtoArticleImage> ArticleImages { get; set; } = new List<DtoArticleImage>();

	[JsonIgnore]
	public bool ReplaceImages { get; set; }
}
