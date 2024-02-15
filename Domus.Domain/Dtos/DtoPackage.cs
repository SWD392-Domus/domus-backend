using Domus.Domain.Dtos.Products;
using Domus.Domain.Entities;

namespace Domus.Domain.Dtos;

public class DtoPackage
{
    public Guid id { get; set; }
    
    public string Name { get; set; } = null!;

    public double Discount { get; set; }

    public ICollection<DtoService> Services { get; set; } = new List<DtoService>();

    public ICollection<DtoProductDetail> ProductDetails { get; set; } = new List<DtoProductDetail>();
    
    public ICollection<DtoPackageImage> PackageImages { get; set; } = new List<DtoPackageImage>();
}