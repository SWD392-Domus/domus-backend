using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Authentication;

public class GoogleLoginRequest : OAuthRequest
{
	[Required]
	public string State { get; set; } = null!;

	[Required]
	public string Code { get; set; } = null!;

	[Required]
	public string Scope { get; set; } = null!;

	[Required]
	public string Authuser { get; set; } = null!;

	[Required]
	public string Hd { get; set; } = null!;

	[Required]
	public string Prompt { get; set; } = null!;
}
