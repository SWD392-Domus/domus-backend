using System.Text.Json.Serialization;

namespace Domus.Domain.Dtos.Products;

public class DtoProductDetailInStorage
{
	public Guid Id { get; set; }

	public double DisplayPrice { get; set; }

	public string ProductName { get; set; } = null!;

	public string? Brand { get; set; }

	public int TotalQuantity { get; set; }

	[JsonPropertyName("attributes")]
    public virtual ICollection<DtoProductAttributeValue> ProductAttributeValues { get; set; } = new List<DtoProductAttributeValue>();

	[JsonPropertyName("images")]
    public virtual ICollection<DtoProductImage> ProductImages { get; set; } = new List<DtoProductImage>();
 
	[JsonPropertyName("prices")]
    public virtual ICollection<DtoProductPrice> ProductPrices { get; set; } = new List<DtoProductPrice>();
    
}
