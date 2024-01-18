using System.ComponentModel.DataAnnotations;
using Domus.Service.Attributes;

namespace Domus.Service.Models.Requests;

public class RegisterRequest
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    [MatchesPattern(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$", "Password is too simple")]
    public string Password { get; set; }
}