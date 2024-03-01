using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class ProductDetailQuotationRevision : BaseEntity<Guid>
{
    public Guid ProductDetailQuotationId { get; set; }

    public int? Version { get; set; }

    public double Price { get; set; }

	public double Quantity { get; set; }

    public virtual ProductDetailQuotation ProductDetailQuotation { get; set; } = null!;
}
