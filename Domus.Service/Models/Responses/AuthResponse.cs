namespace Domus.Service.Models.Responses;

public class AuthResponse
{
	public string Username { get; set; } = null!;
	public IList<string> Roles { get; set; } = new List<string>();
	public TokenResponse Token { get; set; } = null!;
}
