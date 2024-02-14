namespace Domus.Domain.Dtos;

public class DtoQuotation
{
	public string Id { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

	public string CustomerName { get; set; } = null!;

    public string StaffId { get; set; } = null!;

	public string StaffName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? ExpireAt { get; set; }

	public DtoQuotationNegotiationLogWithoutMessages QuotationNegotiationLog { get; set; } = null!;
}
