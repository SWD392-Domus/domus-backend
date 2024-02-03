using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class NegotiationMessage : BaseEntity<Guid>
{
    public DateTime SentAt { get; set; }

    public bool IsCustomerMessage { get; set; }

	public Guid QuotationNegotiationLogId { get; set; }

	public virtual QuotationNegotiationLog QuotationNegotiationLog { get; set; } = null!;
	
    public string? Content { get; set; }
}
