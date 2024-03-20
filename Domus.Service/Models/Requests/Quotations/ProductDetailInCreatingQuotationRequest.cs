using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.Quotations;

public class ProductDetailInCreatingQuotationRequest
{
    [RequiredGuid]
    public Guid Id { get; set; }

	[Range(1f, 999f)]
    public int Quantity { get; set; }

    [Range(0f, 999999999999f)]
    public double Price { get; set; }
}
