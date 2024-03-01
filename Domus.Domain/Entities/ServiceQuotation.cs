namespace Domus.Domain.Entities;

public partial class ServiceQuotation
{
	public Guid QuotationId { get; set; }
	public Guid ServiceId { get; set; }
	public double Price { get; set; }
	public virtual Quotation Quotation { get; set; } = null!;
	public virtual Service Service { get; set; } = null!;
}
