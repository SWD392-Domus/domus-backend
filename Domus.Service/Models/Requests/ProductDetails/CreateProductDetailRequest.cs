using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.ProductDetails;

public class CreateProductDetailRequest
{
	[RequiredGuid]
	public Guid ProductId { get; set; }

	public string? Description { get; set; }

	[Range(0, float.MaxValue)]
	public float Price { get; set; }

	[Range(0, float.MaxValue)]
	public float DisplayPrice { get; set; }

	public string MonetaryUnit { get; set; } = null!;

	public float Quantity { get; set; }

	[JsonIgnore]
	public string QuantityType { get; set; } = null!;

	public ICollection<CreateProductAttributeRequest> Attributes { get; set; } = new List<CreateProductAttributeRequest>();
}

