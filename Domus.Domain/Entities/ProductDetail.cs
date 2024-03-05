using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class ProductDetail : DeletableEntity<Guid>
{
    public Guid ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

	public double DisplayPrice { get; set; }

    public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();

    // public virtual ICollection<ProductDetailQuotation> ProductDetailQuotations { get; set; } = new List<ProductDetailQuotation>();

	public virtual ICollection<ProductDetailQuotationRevision> ProductDetailQuotationRevisions { get; set; } = new List<ProductDetailQuotationRevision>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();

    public virtual ICollection<PackageProductDetail> PackageProductDetail { get; set; } = new List<PackageProductDetail>();
}
