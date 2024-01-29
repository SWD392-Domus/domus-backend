using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Payment;

public abstract class CreatePaymentRequest
{
	[Required]
	public long Amount { get; set; }
	[Required]
	public string BankCode { get; set; }
	[Required]
	public string OrderInfo { get; set; }
	[Required]
	public string CurrCode { get; set; }
	[Required]
	public string IpAddr { get; set; }
	[Required]
	public string Locale { get; set; }
	[Required]
	public string OrderType { get; set; }
}
