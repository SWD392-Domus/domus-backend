using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.Quotations;

public class ProductDetailInUpdatingQuotationRequest
{
    [RequiredGuid]
	[JsonPropertyName("id")]
    public Guid ProductDetailId { get; set; }

	[Range(0f, 999999999999f)]
    public double Price { get; set; }

    public string? MonetaryUnit { get; set; }
    
	[Range(1f, 999f)]
    public int Quantity { get; set; }

    public string? QuantityType { get; set; }
}
