using Domus.Domain.Dtos.Quotations;
using Domus.Domain.Entities;
using Domus.Service.Enums;

namespace Domus.Domain.Dtos;

public class DtoContract
{
    public Guid Id { get; set; }
    public Guid QuotationRevisionId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? SignedAt { get; set; }

    public string? Notes { get; set; }

    public string? Attachments { get; set; }


    public string ClientId { get; set; }
    
    public string ContractorId { get; set; }
    
    public string? Signature { get; set; }
    public string? FullName { get; set; }
    public ContractStatus Status { get; set; }
    public DtoDomusUser Client { get; set; }= null!;
    public DtoDomusUser Contractor { get; set; }= null!;
    public DtoQuotationRevision QuotationRevision { get; set; } = null!;
    public ICollection<DtoServiceQuotation> ServiceQuotations { get; set; } = new List<DtoServiceQuotation>();
}