using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Payment;

namespace Domus.Service.Implementations;

public class VnpayService : IVnpayService
{
	public Task<ServiceActionResult> CreatePaymentUrlAsync(CreatePaymentRequest request)
	{
		throw new NotImplementedException();
	}
}
