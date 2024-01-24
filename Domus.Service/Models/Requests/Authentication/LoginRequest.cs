using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Authentication;

public class LoginRequest
{
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
}
