using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domus.Service.Models.Requests.ProductDetails;

public class UpdateProductPriceRequest
{
    public Guid? Id { get; set; }
    
	[Range(0f, 999999999999f)]
    public double Price { get; set; }

    public string? MonetaryUnit { get; set; }

	[Range(1f, 999f)]
    public double Quantity { get; set; }

	[JsonIgnore]
    public string? QuantityType { get; set; }
}
