namespace Domus.Domain.Dtos.Products;

public class DtoProductAttributeValue
{
	public Guid AttributeId { get; set; }
	
	public string Name { get; set; } = null!;

	public string Value { get; set; } = null!;

	public string ValueType { get; set; } = null!;
}
