namespace Domus.Domain.Dtos;

public class DtoArticleWithoutCategory
{
	public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public ICollection<DtoArticleImage> ArticleImages { get; set; } = new List<DtoArticleImage>();
}
