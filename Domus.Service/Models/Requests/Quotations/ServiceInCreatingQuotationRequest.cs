using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests.Quotations;

public class ServiceInCreatingQuotationRequest
{
	[RequiredGuid]
	public Guid ServiceId { get; set; }

	public double Price { get; set; }
}
