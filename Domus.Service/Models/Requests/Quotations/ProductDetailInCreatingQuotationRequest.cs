using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.Quotations;

public class ProductDetailInCreatingQuotationRequest
{
    [RequiredGuid]
    public Guid ProductDetailId { get; set; }

    public int Quantity { get; set; }
}