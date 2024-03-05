using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;
using Domus.Service.Constants;

namespace Domus.Service.Models.Requests.Users;

public class UpdateUserPasswordRequest
{
    [Required] 
    public string CurrentPassword { get; set; } = null!;

    [Required] 
    [MatchesPattern(PasswordConstants.PasswordPattern, PasswordConstants.PasswordPatternErrorMessage)]
    public string NewPassword { get; set; } = null!;
}