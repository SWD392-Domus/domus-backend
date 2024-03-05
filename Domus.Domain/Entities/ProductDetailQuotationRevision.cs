using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class ProductDetailQuotationRevision : DeletableEntity<Guid>
{
    public Guid QuotationRevisionId { get; set; }

	public Guid ProductDetailId { get; set; }

    public double Price { get; set; }

	public string MonetaryUnit { get; set; } = null!;

	public double Quantity { get; set; }

	public string QuantityType { get; set; } = null!;

    public virtual QuotationRevision QuotationRevision { get; set; } = null!;

	public virtual ProductDetail ProductDetail { get; set; } = null!;
}
