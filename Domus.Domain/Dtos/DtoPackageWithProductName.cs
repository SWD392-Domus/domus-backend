using System.Text.Json.Serialization;
using Domus.Domain.Dtos.Products;
using Domus.Domain.Entities;

namespace Domus.Domain.Dtos;

public class DtoPackageWithProductName 
{
    public Guid id { get; set; }
    
    public string Name { get; set; } = null!;

    public double Discount { get; set; }
    
    public double EstimatedPrice { get; set; }

    public string? Description { get; set; } 

    public ICollection<DtoService> Services { get; set; } = new List<DtoService>();
    [JsonPropertyName("productDetails")]
    public ICollection<DtoPackageProductDetail> PackageProductDetails { get; set; } = new List<DtoPackageProductDetail>();
    
    public ICollection<DtoPackageImage> PackageImages { get; set; } = new List<DtoPackageImage>();
}