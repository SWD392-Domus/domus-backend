using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.ProductDetails;

public class UpdateProductAttributeValueRequest
{
    public Guid? AttributeId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Value { get; set; } = null!;

    [Required]
    public string ValueType { get; set; } = null!;
}