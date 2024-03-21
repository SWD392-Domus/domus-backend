using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.Quotations;

public class ServiceInUpdatingQuotationRequest 
{
    [RequiredGuid]
	public Guid ServiceId { get; set; }

	[Range(0f, 999999999999f)]
	public double Price { get; set; }
}
