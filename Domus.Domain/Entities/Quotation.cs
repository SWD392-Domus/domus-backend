using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Quotation : TrackableEntity<string>
{
    public Guid Id { get; set; }

    public string CustomerId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string? Status { get; set; }

    public virtual DomusUser Customer { get; set; } = null!;

    public virtual ICollection<ProductDetailQuotation> ProductDetailQuotations { get; set; } = new List<ProductDetailQuotation>();

    public virtual DomusUser Staff { get; set; } = null!;
}
