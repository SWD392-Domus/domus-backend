using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domus.Service.Models.Requests.ProductDetails;

public class CreateProductPriceRequest
{
	[Required]
	[Range(0, double.MaxValue)]
	public double Price { get; set; }

	[Required]
	public string MonetaryUnit { get; set; } = null!;

	[Range(0, double.MaxValue)]
	public double Quantity { get; set; }

	// [Required]
	[JsonIgnore]
	public string QuantityType { get; set; } = string.Empty;
}
