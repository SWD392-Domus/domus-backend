using System.Text.Json.Serialization;

namespace Domus.Service.Models.Requests.Quotations;

public class UpdateQuotationRequest
{
    public string? CustomerId { get; set; }

    public string? StaffId { get; set; }

    public string? Status { get; set; }

    public DateTime? ExpireAt { get; set; }

    [JsonPropertyName("productdetails")]
    public ICollection<ProductDetailInUpdatingQuotationRequest> ProductDetailQuotations { get; set; } = new List<ProductDetailInUpdatingQuotationRequest>();

    public ICollection<ServiceInUpdatingQuotationRequest> Services { get; set; } = new List<ServiceInUpdatingQuotationRequest>();
}
