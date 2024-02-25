using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.Quotations;

public class ProductDetailInUpdatingQuotationRequest
{
    [RequiredGuid]
    public Guid ProductDetailId { get; set; }

    public double Price { get; set; }

    public string? MonetaryUnit { get; set; }
    
    public int Quantity { get; set; }

    public string? QuantityType { get; set; }
}