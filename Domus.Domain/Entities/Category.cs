using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Category : TrackableEntity<string>
{
    public Guid Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
