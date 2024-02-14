namespace Domus.Domain.Dtos.Quotations;

public class DtoQuotationNegotiationLogWithoutMessages
{
    public bool? IsClosed { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime? CloseAt { get; set; }
}
