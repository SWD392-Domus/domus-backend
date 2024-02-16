using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domus.Service.Models.Requests.Authentication;

public class FacebookLoginRequest : OAuthRequest
{
	[Required]
	[JsonPropertyName("access_token")]
	public string AccessToken { get; set; } = null!;

	[JsonPropertyName("data_access_expiration_time")]
	public long DataAccessExpirationTime { get; set; }

	[JsonPropertyName("expires_in")]
	public int ExpiresIn { get; set; }
}
