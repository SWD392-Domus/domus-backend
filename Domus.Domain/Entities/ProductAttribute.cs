using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class ProductAttribute : BaseEntity<Guid>
{
    public string AttributeName { get; set; } = null!;

    public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();
}