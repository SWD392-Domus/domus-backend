using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class QuotationRevision : DeletableEntity<Guid>
{
	public Guid QuotationId { get; set; }
	public int Version { get; set; }
	public DateTime CreatedAt { get; set; }
	public virtual Quotation Quotation { get; set; } = null!;
	public virtual ICollection<ProductDetailQuotationRevision> ProductDetailQuotationRevisions { get; set; } = new List<ProductDetailQuotationRevision>();
}
