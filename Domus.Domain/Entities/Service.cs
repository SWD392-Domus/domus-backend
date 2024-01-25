using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Service : DeletableEntity<Guid>
{
    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public string MonetaryUnit { get; set; } = null!;

    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
}
