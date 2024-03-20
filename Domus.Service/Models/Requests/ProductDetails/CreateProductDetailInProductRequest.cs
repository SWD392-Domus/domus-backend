using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domus.Service.Models.Requests.ProductDetails;

public class CreateProductDetailInProductRequest
{
    public string? Description { get; set; }

	[Range(0f, 999999999999f)]
    public float DisplayPrice { get; set; }

    public string MonetaryUnit { get; set; } = null!;

	[Range(1f, 999f)]
    public float Quantity { get; set; }

	[JsonIgnore]
    public string QuantityType { get; set; } = string.Empty;

    [JsonPropertyName("attributes")]
    public ICollection<CreateProductAttributeRequest> ProductAttributeValues { get; set; } = new List<CreateProductAttributeRequest>();
    
    public ICollection<CreateProductPriceRequest> ProductPrices { get; set; } = new List<CreateProductPriceRequest>();
}
