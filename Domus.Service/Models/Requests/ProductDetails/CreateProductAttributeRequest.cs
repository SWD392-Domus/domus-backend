using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.ProductDetails;

public class CreateProductAttributeRequest
{
	[Required]
	public string Name { get; set; } = null!;

	[Required]
	public string Value { get; set; } = null!;

	[Required]
	public string ValueType { get; set; } = null!;
}
