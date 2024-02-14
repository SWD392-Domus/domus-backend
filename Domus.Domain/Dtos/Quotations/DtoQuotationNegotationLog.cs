namespace Domus.Domain.Dtos.Quotations;

public class DtoQuotationNegotiationLog
{
    public bool? IsClosed { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime? CloseAt { get; set; }

	public ICollection<DtoNegotiationMessage> NegotiationMessages { get; set; } = null!;
}
