namespace Domus.Domain.Dtos.Quotations;

public class DtoQuotationFullDetails
{
	public string Id { get; set; } = null!;

	public DtoDomusUser Customer { get; set; } = null!;

	public DtoDomusUser Staff { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? ExpireAt { get; set; }

	public ICollection<DtoProductDetailQuotation> Products { get; set; } = null!;

	public DtoQuotationNegotiationLog QuotationNegotiationLog { get; set; } = null!;
}
