namespace Domus.Service.Models.Requests.Contracts;

public class ContractRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Notes { get; set; }
    public string? Attachments { get; set; }
    public string ClientId { get; set; }
    public string ContractorId { get; set; }
    public Guid QuotationRevisionId { get; set; }
    public string? Signature { get; set; }
}