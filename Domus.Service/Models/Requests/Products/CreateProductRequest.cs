using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.Products;

public class CreateProductRequest 
{
	[RequiredGuid]
    public Guid ProductCategoryId { get; set; }

	[Required]
    public string ProductName { get; set; } = null!;

    public string? Color { get; set; }

    public double Weight { get; set; }

    public string? WeightUnit { get; set; }

    public string? Style { get; set; }

    public string? Brand { get; set; }

    public string? Description { get; set; }
}
