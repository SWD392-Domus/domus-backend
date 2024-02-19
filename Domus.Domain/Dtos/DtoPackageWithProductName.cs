using Domus.Domain.Dtos.Products;

namespace Domus.Domain.Dtos;

public class DtoPackageWithProductName 
{
    public Guid id { get; set; }
    
    public string Name { get; set; } = null!;

    public double Discount { get; set; }
    
    public double EstimatedPrice { get; set; } 

    public ICollection<DtoService> Services { get; set; } = new List<DtoService>();

    public ICollection<DtoProductDetailPackage> ProductDetails { get; set; } = new List<DtoProductDetailPackage>();
    
    public ICollection<DtoPackageImage> PackageImages { get; set; } = new List<DtoPackageImage>();
}