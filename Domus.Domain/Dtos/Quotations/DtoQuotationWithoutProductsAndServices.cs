namespace Domus.Domain.Dtos.Quotations;

public class DtoQuotationWithoutProductsAndServices
{
    public Guid Id { get; set; }

    public DtoDomusUser Customer { get; set; } = null!;

    public DtoDomusUser Staff { get; set; } = null!;

    public string Status { get; set; } = null!;

    public double TotalPrice { get; set; }

    public DateTime? ExpireAt { get; set; }
}