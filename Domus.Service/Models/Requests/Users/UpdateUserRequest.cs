namespace Domus.Service.Models.Requests.Users;

public class UpdateUserRequest
{
	public string? Email { get; set; }

	public string? UserName { get; set; }

	public string? PhoneNumber { get; set; }

	public string? ProfileImage { get; set; }
}
