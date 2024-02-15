using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Product : BaseEntity<Guid>
{
    public Guid ProductCategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Brand { get; set; }

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ProductCategory ProductCategory { get; set; } = null!;

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
