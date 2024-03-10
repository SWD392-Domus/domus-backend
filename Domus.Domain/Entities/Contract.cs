using Domus.Domain.Entities.Base;
using Domus.Service.Enums;

namespace Domus.Domain.Entities;

public partial class Contract : BaseEntity<Guid>
{
    public Guid QuotationRevisionId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }

    public string? Notes { get; set; }

    public string? Attachments { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string ClientId { get; set; }
    
    public string ContractorId { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public string? Signature { get; set; }
    
    public ContractStatus  Status { get; set; }
    
    public virtual DomusUser Client { get; set; }= null!;
    public virtual DomusUser Contractor { get; set; }= null!;
    public virtual QuotationRevision QuotationRevision { get; set; } = null!;
}