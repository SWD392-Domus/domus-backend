using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domus.Service.Models.Requests.ProductDetails;

public class UpdateProductAttributeValueRequest
{
    public Guid? AttributeId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Value { get; set; } = null!;

    // [Required]
	[JsonIgnore]
    public string ValueType { get; set; } = string.Empty;
}
