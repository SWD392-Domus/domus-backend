using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Quotation : TrackableEntity<Guid, string>
{
    public string CustomerId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public Guid? PackageId { get; set; } 

    public string Status { get; set; } = null!;

    public DateTime? ExpireAt { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual DomusUser Customer { get; set; } = null!;

    public virtual ICollection<ProductDetailQuotation> ProductDetailQuotations { get; set; } = new List<ProductDetailQuotation>();

    public virtual ICollection<ServiceQuotation> ServiceQuotations { get; set; } = new List<ServiceQuotation>();

    public virtual QuotationNegotiationLog QuotationNegotiationLog { get; set; } = null!;

    public virtual DomusUser Staff { get; set; } = null!;

    public virtual Package? Package { get; set; }
}
