using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Product : TrackableEntity<string>
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Color { get; set; }

    public double? Weight { get; set; }

    public string? WeightUnit { get; set; }

    public string? Style { get; set; }

    public string? Brand { get; set; }

    public string? Description { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
