using System.Text.Json.Serialization;
using Domus.Domain.Dtos.Products;

namespace Domus.Domain.Dtos;

public class DtoPackageProductDetail 
{
    public Guid Id { get; set; }
    
    public int Quantity { get; set;}
    public string ProductName { get; set; }

    public double DisplayPrice { get; set; }

    [JsonPropertyName("attributes")]
    public virtual ICollection<DtoProductAttributeValue> ProductAttributeValues { get; set; } = new List<DtoProductAttributeValue>();

    [JsonPropertyName("images")]
    public virtual ICollection<DtoProductImage> ProductImages { get; set; } = new List<DtoProductImage>();
}