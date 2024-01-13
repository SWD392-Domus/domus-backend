namespace Domus.Domain.Entities;

public partial class ProductAttribute
{
    public Guid Id { get; set; }

    public string AttributeName { get; set; } = null!;

    public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();
}
