using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Article : TrackableEntity<string>
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;
}
