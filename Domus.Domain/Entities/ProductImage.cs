using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class ProductImage : BaseEntity<Guid>
{
    public Guid ProductDetailId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int? Width { get; set; }

    public int? Height { get; set; }

    public virtual ProductDetail ProductDetail { get; set; } = null!;
}