using System.Text.Json.Serialization;

namespace Domus.Domain.Dtos.Quotations;

public class DtoQuotationFullDetails
{
	public Guid Id { get; set; }

	public DtoDomusUser Customer { get; set; } = null!;

	public DtoDomusUser Staff { get; set; } = null!;

    public string Status { get; set; } = null!;

	public double TotalPrice { get; set; }

    public DateTime? ExpireAt { get; set; }

	[JsonPropertyName("products")]
	// public ICollection<DtoProductDetailQuotation> ProductDetailQuotations { get; set; } = new List<DtoProductDetailQuotation>();
	public ICollection<DtoProductDetailQuotationRevision> ProductDetailQuotations { get; set; } = new List<DtoProductDetailQuotationRevision>();

	[JsonPropertyName("services")]
	public ICollection<DtoServiceQuotation> ServiceQuotations { get; set; } = new List<DtoServiceQuotation>();

	[JsonPropertyName("negotiationLog")]
	public DtoQuotationNegotiationLog QuotationNegotiationLog { get; set; } = null!;
}
