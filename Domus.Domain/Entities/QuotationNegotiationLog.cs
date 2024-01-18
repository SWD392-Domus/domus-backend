using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class QuotationNegotiationLog : BaseEntity<Guid>
{
    public Guid QuotationId { get; set; }

    public bool? IsClosed { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime? CloseAt { get; set; }

    public virtual Quotation Quotation { get; set; } = null!;

    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}