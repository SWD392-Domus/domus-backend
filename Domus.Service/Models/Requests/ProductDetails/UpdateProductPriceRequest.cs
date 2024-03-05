using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domus.Service.Models.Requests.ProductDetails;

public class UpdateProductPriceRequest
{
    public Guid? Id { get; set; }
    
	[Range(0, double.MaxValue)]
    public double Price { get; set; }

    public string? MonetaryUnit { get; set; }

	[Range(0, double.MaxValue)]
    public double Quantity { get; set; }

	[JsonIgnore]
    public string? QuantityType { get; set; }
}
