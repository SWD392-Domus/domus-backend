using Domus.Service.Attributes;
using Domus.Service.Models.Requests.ProductDetails;

namespace Domus.Service.Models.Requests.Products;

public class UpdateProductRequest 
{
	[RequiredGuid]
    public Guid ProductCategoryId { get; set; }

    public string? ProductName { get; set; }

    public string? Brand { get; set; }

    public string? Description { get; set; }

    public ICollection<UpdateProductDetailRequest> ProductDetails { get; set; } = new List<UpdateProductDetailRequest>();
}