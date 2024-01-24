using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests;

public class CreateProductPriceRequest
{
	[RequiredGuid]
	public Guid ProductId { get; set; }

	[RequiredGuid]
	public Guid ProductDetailId { get; set; }

	[Required]
	public decimal Price { get; set; }

	[Required]
	public DateTime StartDate { get; set; }

	public DateTime? EndDate { get; set; }
}
