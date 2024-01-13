namespace Domus.Domain.Entities;

public partial class ProductPrice
{
    public Guid Id { get; set; }

    public Guid ProductDetailId { get; set; }

    public double Price { get; set; }

    public string MonetaryUnit { get; set; } = null!;

    public virtual ProductDetail ProductDetail { get; set; } = null!;
}
