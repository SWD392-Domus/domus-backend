using Domus.Domain.Entities;

namespace Domus.Entities;

public class QuotationService
{
	public Guid QuotationId { get; set; }
	public Guid ServiceId { get; set; }
	public double Price { get; set; }
	public virtual Quotation Quotation { get; set; } = null!;
	public virtual Service Service { get; set; } = null!;
}
