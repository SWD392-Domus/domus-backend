using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class PackageImage : BaseEntity<Guid>
{
    public Guid PackageId { get; set; }
    
    public string ImageUrl { get; set; } = null!;

    public int? Width { get; set; }

    public int? Height { get; set; }
    
    public virtual Package Package { get; set; } = null!;
}