namespace Domus.Domain.Dtos;

public class DtoQuotationNegotiationLogWithoutMessages
{
    public bool? IsClosed { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime? CloseAt { get; set; }
}
