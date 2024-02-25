using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Quotations;

public class CreateNegotiationMessageRequest
{
	[Required]
	public string Content { get; set; } = null!;

	public bool IsCustomerMessage { get; set; }
}
