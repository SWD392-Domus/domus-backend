using Domus.Domain.Dtos.Products;

namespace Domus.Domain.Dtos.Quotations;

public class DtoProductDetailQuotationRevision
{
	public string ProductName { get; set; } = null!;

    public double Price { get; set; }

    public string MonetaryUnit { get; set; } = null!;

    public double Quantity { get; set; }

    public string QuantityType { get; set; } = null!;

    public DtoProductDetail Detail { get; set; } = null!;
}
