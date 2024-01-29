using Domus.Service.Models;
using Domus.Service.Models.Requests.Payment;

namespace Domus.Service.Interfaces;

public interface IPaymentService
{
	Task<ServiceActionResult> CreatePaymentUrlAsync(CreatePaymentRequest request);
}
