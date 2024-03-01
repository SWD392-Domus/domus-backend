using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.Quotations;

public class ProductDetailInCreatingQuotationRequest
{
    [RequiredGuid]
    public Guid Id { get; set; }

	[Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}
