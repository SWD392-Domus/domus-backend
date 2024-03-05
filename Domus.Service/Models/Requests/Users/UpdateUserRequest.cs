using Microsoft.AspNetCore.Http;

namespace Domus.Service.Models.Requests.Users;

public class UpdateUserRequest
{
	public string? Email { get; set; }

	public string? UserName { get; set; }

	public string? FullName { get; set; }

	public string? Gender { get; set; }

	public string? Address { get; set; }

	public string? PhoneNumber { get; set; }

	public IFormFile? ProfileImage { get; set; }
}
