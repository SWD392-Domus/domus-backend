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
	public long Vnp_TransactionNo { get; set; }

	[Required]
	public long Vnp_ResponseCode { get; set; }

	[Required]
	public int Vnp_TransactionStatus { get; set; }

	[Required]
	public string Vnp_TxnRef { get; set; } = null!;

	[Required]
	public string Vnp_SecureHash { get; set; } = null!;

	public string? Vnp_SecureHashType { get; set; }

	public string? Vnp_BankTranNo { get; set; }

	public string? Vnp_CardType { get; set; }

	public long Vnp_PayDate { get; set; }
}
