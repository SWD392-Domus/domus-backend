using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class QuotationStatus : BaseEntity<Guid>
{
    public string StatusType { get; set; } = null!;
}