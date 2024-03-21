using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domus.Service.Models.Requests.ProductDetails;

public class UpdateProductDetailRequest
{
	public Guid Id { get; set; }
	
	[Range(0f, 999999999999f)]
	public double DisplayPrice { get; set; }

	[JsonPropertyName("attributes")]
	public virtual ICollection<UpdateProductAttributeValueRequest> ProductAttributeValues { get; set; } = new List<UpdateProductAttributeValueRequest>();

	[JsonPropertyName("prices")]
	public virtual ICollection<UpdateProductPriceRequest> ProductPrices { get; set; } = new List<UpdateProductPriceRequest>();
}
