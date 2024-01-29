using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Payment;

public class VnpayPaymentResponse : PaymentResponse
{
	[Required]
    public string Vnp_TmnCode { get; set; } = null!;

	[Required]
	public long Vnp_Amount { get; set; }

	[Required]
	public string Vnp_BankCode { get; set; } = null!;

	[Required]
	public string Vnp_OrderInfo { get; set; } = null!;

	[Required]
	public string Vnp_TransactionNo { get; set; } = null!;

	[Required]
	public string Vnp_ResponseCode { get; set; } = null!;

	[Required]
	public string Vnp_TransactionStatus { get; set; } = null!;

	[Required]
	public string Vnp_TxnRef { get; set; } = null!;

	[Required]
	public string Vnp_SecureHash { get; set; } = null!;

	public string? Vnp_SecureHashType { get; set; }

	public string? Vnp_BankTranNo { get; set; }

	public string? Vnp_CardType { get; set; }

	public string? Vnp_PayDate { get; set; }
}
