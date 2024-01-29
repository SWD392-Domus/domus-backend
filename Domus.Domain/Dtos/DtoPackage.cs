namespace Domus.Domain.Dtos;

public class DtoPackage
{
    public string Name { get; set; } = null!;

    public double Discount { get; set; }

    public virtual ICollection<DtoService> Services { get; set; } = new List<DtoService>();

    public virtual ICollection<DtoProductDetail> ProductDetails { get; set; } = new List<DtoProductDetail>();
}