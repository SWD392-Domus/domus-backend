using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.Quotations;

public class ServiceInCreatingQuotationRequest
{
	[RequiredGuid]
	public Guid ServiceId { get; set; }

	[Range(0, double.MaxValue)]
	public double Price { get; set; }
}
