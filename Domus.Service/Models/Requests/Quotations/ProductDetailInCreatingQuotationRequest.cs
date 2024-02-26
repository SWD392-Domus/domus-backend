using Domus.Service.Attributes;
using Newtonsoft.Json;

namespace Domus.Service.Models.Requests.Quotations;

public class ProductDetailInCreatingQuotationRequest
{
    [RequiredGuid]
    public Guid Id { get; set; }

    public int Quantity { get; set; }
}