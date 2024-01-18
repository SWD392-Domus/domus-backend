using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class ArticleCategory : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}