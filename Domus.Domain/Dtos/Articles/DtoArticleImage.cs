namespace Domus.Domain.Dtos.Articles;

public class DtoArticleImage
{
	public Guid Id { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int Width { get; set; }

    public int Height { get; set; }
}
