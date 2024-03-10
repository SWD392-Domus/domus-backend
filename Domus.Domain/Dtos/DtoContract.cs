using Domus.Domain.Entities;

namespace Domus.Domain.Dtos;

public class DtoContract
{
    public Guid Id { get; set; }
    public Guid QuotationRevisionId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }

    public string? Notes { get; set; }

    public string? Attachments { get; set; }


    public string ClientId { get; set; }
    
    public string ContractorId { get; set; }
    
    public string? Signature { get; set; }
    
    public  DtoDomusUser Client { get; set; }= null!;
    public  DtoDomusUser Contractor { get; set; }= null!;
    public  QuotationRevision QuotationRevision { get; set; } = null!;
}