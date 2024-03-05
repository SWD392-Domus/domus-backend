namespace Domus.Domain.Dtos.Quotations;

public class DtoServiceQuotation
{
    public Guid QuotationId { get; set; }
    public Guid ServiceId { get; set; }
    public string Name { get; set; } = null!;
    public double Price { get; set; }
}