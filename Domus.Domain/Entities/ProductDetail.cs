namespace Domus.Domain.Entities;

public partial class ProductDetail
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public double Quantity { get; set; }

    public string QuantityType { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();

    public virtual ICollection<ProductDetailQuotation> ProductDetailQuotations { get; set; } = new List<ProductDetailQuotation>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
}
