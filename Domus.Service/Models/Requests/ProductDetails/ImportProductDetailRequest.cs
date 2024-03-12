using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.ProductDetails;

public class ImportProductDetailRequest 
{
	[RequiredGuid]
	public Guid ProductDetailId { get; set; }

	[Required]
	[Range(0, double.MaxValue)]
	public double Price { get; set; }

	[Required]
	public string MonetaryUnit { get; set; } = null!;

	[Range(0, double.MaxValue)]
	public double Quantity { get; set; }
}
