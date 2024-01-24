using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.ProductDetails;

public class CreateProductDetailRequest
{
	[RequiredGuid]
	public Guid ProductId { get; set; }

	[Required]
	public string Name { get; set; } = null!;

	public string? Description { get; set; }

	public float Price { get; set; }

	public float DisplayPrice { get; set; }

	public string MonetaryUnit { get; set; } = null!;

	public float Quantity { get; set; }

	public string QuantityType { get; set; } = null!;

	public ICollection<CreateProductAttributeRequest> Attributes { get; set; } = new List<CreateProductAttributeRequest>();
}

