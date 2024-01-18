using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Article : TrackableEntity<Guid, string>
{
    public Guid ArticleCategoryId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? ConcurrencyStamp { get; set; }

    public virtual ArticleCategory ArticleCategory { get; set; } = null!;

    public virtual ICollection<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();
}