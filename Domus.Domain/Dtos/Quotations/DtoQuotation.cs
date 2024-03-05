namespace Domus.Domain.Dtos.Quotations;

public class DtoQuotation
{
	public Guid Id { get; set; }

    public string CustomerId { get; set; } = null!;

	public string CustomerName { get; set; } = null!;

    public string StaffId { get; set; } = null!;

	public string StaffName { get; set; } = null!;

    public string Status { get; set; } = null!;

	public float TotalPrice { get; set; }

    public DateTime? ExpireAt { get; set; }

	public DtoQuotationNegotiationLogWithoutMessages QuotationNegotiationLog { get; set; } = null!;
}
