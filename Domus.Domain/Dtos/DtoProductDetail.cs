namespace Domus.Domain.Dtos;

public class DtoProductDetail
{
	public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = null!;

	public double DisplayPrice { get; set; }

    public virtual ICollection<DtoProductAttributeValue> ProductAttributeValues { get; set; } = new List<DtoProductAttributeValue>();

    public virtual ICollection<DtoProductImage> ProductImages { get; set; } = new List<DtoProductImage>();
}
