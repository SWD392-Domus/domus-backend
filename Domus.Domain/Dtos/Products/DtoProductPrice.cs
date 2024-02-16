namespace Domus.Domain.Dtos.Products;

public class DtoProductPrice
{
	public double Price { get; set; }

	public string MonetaryUnit { get; set; } = null!;

	public double Quantity { get; set; }

	public string QuantityType { get; set; } = null!;
}
