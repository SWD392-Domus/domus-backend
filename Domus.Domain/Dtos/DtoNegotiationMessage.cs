namespace Domus.Domain.Dtos;

public class DtoNegotiationMessage
{
	public Guid Id { get; set; }

    public DateTime SentAt { get; set; }

    public bool IsCustomerMessage { get; set; }

	public Guid QuotationNegotiationLogId { get; set; }
	
    public string? Content { get; set; }
}
