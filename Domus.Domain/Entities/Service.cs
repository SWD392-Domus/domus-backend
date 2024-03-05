using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Service : DeletableEntity<Guid>
{
    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public string MonetaryUnit { get; set; } = null!;

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();

    public virtual ICollection<ServiceQuotation> ServiceQuotations { get; set; } = new List<ServiceQuotation>();
}
