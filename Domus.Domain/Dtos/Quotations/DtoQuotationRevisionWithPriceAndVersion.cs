namespace Domus.Domain.Dtos.Quotations;

public class DtoQuotationRevisionWithPriceAndVersion
{
	public Guid Id { get; set; }
	public int Version { get; set; }
	public double TotalPrice { get; set; }
}
