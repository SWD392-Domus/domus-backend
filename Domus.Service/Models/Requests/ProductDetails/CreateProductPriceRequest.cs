using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.ProductDetails;

public class CreateProductPriceRequest
{
	[Required]
	public double Price { get; set; }

	[Required]
	public string MonetaryUnit { get; set; } = null!;

	public double Quantity { get; set; }

	[Required]
	public string QuantityType { get; set; } = null!;
}
