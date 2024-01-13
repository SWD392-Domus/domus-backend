using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Contract : TrackableEntity<string>
{
    public Guid Id { get; set; }

    public Guid QuotationId { get; set; }

    public string? Status { get; set; }

    public DateTime? SignedAt { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Notes { get; set; }

    public string? Attachments { get; set; }
}
