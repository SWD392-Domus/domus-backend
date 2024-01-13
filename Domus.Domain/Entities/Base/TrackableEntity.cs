namespace Domus.Domain.Entities.Base;

public abstract class TrackableEntity<TUserKey> : BaseEntity
{
    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public TUserKey CreatedBy { get; set; }

    public TUserKey? LastUpdatedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual DomusUser CreatedByNavigation { get; set; } = null!;

    public virtual DomusUser? LastUpdatedByNavigation { get; set; }
}
