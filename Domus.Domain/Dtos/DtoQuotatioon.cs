namespace Domus.Domain.Dtos;

public class DtoQuotation
{
    public string CustomerId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? ExpireAt { get; set; }
}
