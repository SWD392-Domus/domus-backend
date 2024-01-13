namespace Domus.Domain.Entities;

public partial class ProductDetailQuotationRevision
{
    public Guid Id { get; set; }

    public Guid ProductDetailQuotationId { get; set; }

    public int? Version { get; set; }

    public double Price { get; set; }

    public virtual ProductDetailQuotation ProductDetailQuotation { get; set; } = null!;
}
