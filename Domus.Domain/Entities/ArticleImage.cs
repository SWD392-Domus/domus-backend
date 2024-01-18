using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class ArticleImage : BaseEntity<Guid>
{
    public Guid ArticleId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int? Width { get; set; }

    public int? Height { get; set; }

    public virtual Article Article { get; set; } = null!;
}