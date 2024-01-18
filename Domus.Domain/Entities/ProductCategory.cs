using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class ProductCategory : DeletableEntity<Guid>
{
    public string Name { get; set; } = null!;

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}