namespace Domus.Domain.Dtos.Quotations;

public class DtoQuotationRevision
{
    public Guid QuotationId { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public double TotalPrice { get; set; }
    public virtual DtoQuotation Quotation { get; set; } = null!;
    public virtual ICollection<DtoProductDetailQuotationRevision> ProductDetailQuotationRevisions { get; set; } = new List<DtoProductDetailQuotationRevision>();
}