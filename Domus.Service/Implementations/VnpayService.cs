using Domus.Common.Constants;
using Domus.Common.Exceptions;
using Domus.Common.Helpers;
using Domus.Common.Models.Common;
using Domus.Common.Settings;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Payment;
using Microsoft.Extensions.Configuration;

namespace Domus.Service.Implementations;

public class VnpayService : IVnpayService
{
	private readonly IConfiguration _configuration;

	public VnpayService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task<ServiceActionResult> CreatePaymentUrlAsync(CreatePaymentRequest request)
	{
		var vnpaySettings = _configuration.GetSection(nameof(VnpaySettings)).Get<VnpaySettings>() ?? throw new MissingVnpaySettingsException();
		var createDate = DateTime.Now.ToString(VnpayConstants.DATE_FORMAT);
		var amountAsString = (request.Amount * 100).ToString();
		var txnRefAsString = (new Random().Next()).ToString();
		var requestData = new SortedList<string, string>(new VnpayParamComparer());

		requestData.Add(VnpayConstants.VERSION, vnpaySettings.VnpayVersion);
		requestData.Add(VnpayConstants.COMMAND, VnpayConstants.COMMAND_PAY);
		requestData.Add(VnpayConstants.TMN_CODE, vnpaySettings.VnpayTmnCode);
		requestData.Add(VnpayConstants.RETURN_URL, vnpaySettings.VnpayReturnUrl);
		requestData.Add(VnpayConstants.CREATE_DATE, createDate);
		requestData.Add(VnpayConstants.AMOUNT, amountAsString);
		requestData.Add(VnpayConstants.CURRENCY_CODE, request.CurrCode);
		requestData.Add(VnpayConstants.TXN_REF, txnRefAsString);
		requestData.Add(VnpayConstants.ORDER_INFO, request.OrderInfo);
		requestData.Add(VnpayConstants.ORDER_TYPE, request.OrderType);
		requestData.Add(VnpayConstants.LOCALE, request.Locale);
		requestData.Add(VnpayConstants.IP_ADDRESS, request.IpAddr);

		string queryString = VnpayHelper.BuildQueryString(requestData);

		await Task.CompletedTask;
		return new ServiceActionResult(true) { Data = VnpayHelper.BuildPaymentUrl(vnpaySettings.VnpayUrl, vnpaySettings.VnpaySecretKey, queryString) };
	}

	public async Task<ServiceActionResult> ProcessPaymentResponse(PaymentResponse response)
	{
		var vnpayResponse = (VnpayPaymentResponse)response;

		await Task.CompletedTask;
		return new ServiceActionResult(true);
	}
}
