using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;
using Domus.Service.Constants;

namespace Domus.Service.Models.Requests.Authentication;

public class RegisterRequest
{
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    [MatchesPattern(PasswordConstants.PasswordPattern, PasswordConstants.PasswordPatternErrorMessage)]
    public string Password { get; set; } = null!;
}
