namespace Domus.Domain.Entities.Base;

public abstract class DeletableEntity<TKey> : BaseEntity<TKey>
{
    public bool IsDeleted { get; set; } 
}