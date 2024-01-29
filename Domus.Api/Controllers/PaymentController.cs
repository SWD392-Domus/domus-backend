using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Payment;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.controllers;

[Route("api/[controller]")]
public class PaymentController : BaseApiController
{
	private readonly IVnpayService _vnpayService;

	public PaymentController(IVnpayService vnpayService)
	{
		_vnpayService = vnpayService;
	}

	[HttpPost("vnpay/create-payment-url")]
	public async Task<IActionResult> CreatePaymentUrl(CreatePaymentRequest request)
	{
		return await ExecuteServiceLogic(
				async () => await _vnpayService.CreatePaymentUrlAsync(request).ConfigureAwait(false))
		).ConfigureAwait(false);
	}
}
