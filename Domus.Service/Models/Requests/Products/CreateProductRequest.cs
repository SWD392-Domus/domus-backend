using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;
using Domus.Service.Models.Requests.ProductDetails;
using Newtonsoft.Json.Linq;

namespace Domus.Service.Models.Requests.Products;

public class CreateProductRequest 
{
	[RequiredGuid]
    public Guid ProductCategoryId { get; set; }

	[Required]
    public string ProductName { get; set; } = null!;

    public string? Brand { get; set; }

    public string? Description { get; set; }

    public ICollection<CreateProductDetailInProductRequest> ProductDetails { get; set; } = new List<CreateProductDetailInProductRequest>();
}
