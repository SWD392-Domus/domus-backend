using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public class PackageProductDetail 
{
    public Guid PackageId { get; set; }
    public Guid ProductDetailId { get; set; }
    public int Quantity { get; set; }
    public virtual Package Package { get; set; } = null!;
    public virtual ProductDetail ProductDetail { get; set; } = null!;
}