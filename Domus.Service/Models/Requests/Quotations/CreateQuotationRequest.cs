namespace Domus.Service.Models.Requests.Quotations;

public class CreateQuotationRequest
{
    public DateTime? ExpireAt { get; set; }

    public Guid? PackageId { get; set; }

    public ICollection<ProductDetailInCreatingQuotationRequest> ProductDetails { get; set; } = new List<ProductDetailInCreatingQuotationRequest>();

    public ICollection<Guid> Services { get; set; } = new List<Guid>();
}
