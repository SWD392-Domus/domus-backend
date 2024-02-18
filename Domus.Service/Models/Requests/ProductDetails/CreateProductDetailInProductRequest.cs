using System.Text.Json.Serialization;

namespace Domus.Service.Models.Requests.ProductDetails;

public class CreateProductDetailInProductRequest
{
    public string? Description { get; set; }

    public float DisplayPrice { get; set; }

    public string MonetaryUnit { get; set; } = null!;

    public float Quantity { get; set; }

    public string QuantityType { get; set; } = null!;

    [JsonPropertyName("attributes")]
    public ICollection<CreateProductAttributeRequest> ProductAttributeValues { get; set; } = new List<CreateProductAttributeRequest>();
    
    public ICollection<CreateProductPriceRequest> ProductPrices { get; set; } = new List<CreateProductPriceRequest>();
}