using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class ProductAttributeValue : BaseEntity<Guid>
{
    public Guid ProductAttributeId { get; set; }

    public Guid ProductDetailId { get; set; }

    public string Value { get; set; } = null!;

    public string ValueType { get; set; } = null!;

    public virtual ProductAttribute ProductAttribute { get; set; } = null!;

    public virtual ProductDetail ProductDetail { get; set; } = null!;
}