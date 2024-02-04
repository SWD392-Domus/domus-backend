using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;
using Domus.Service.Constants;

namespace Domus.Service.Models.Requests.Users;

public class CreateUserRequest
{
    [Required]
    public string Email { get; set; } = null!;

	[Required]
	public string UserName { get; set; } = null!;

	public string? PhoneNumber { get; set; }

	public string? ProfileImage { get; set; }
    
    [Required]
    [MatchesPattern(PasswordConstants.PasswordPattern, PasswordConstants.PasswordPatternErrorMessage)]
    public string Password { get; set; } = null!;
}
