namespace Domus.Domain.Entities.Base;

public abstract class BaseEntity
{
    public string? ConcurrencyStamp { get; set; }
}
