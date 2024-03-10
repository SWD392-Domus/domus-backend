using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public class Otp : BaseEntity<Guid>
{
    public string Code { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool Used { get; set; }
    public string UserId { get; set; } = null!;
    public DomusUser User { get; set; } = null!;
}