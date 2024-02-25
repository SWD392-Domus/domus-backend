using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domus.Service.Models.Requests.ProductDetails;

public class CreateProductPriceRequest
{
	[Required]
	public double Price { get; set; }

	[Required]
	public string MonetaryUnit { get; set; } = null!;

	public double Quantity { get; set; }

	// [Required]
	[JsonIgnore]
	public string QuantityType { get; set; } = string.Empty;
}
