using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests;

public class RegisterRequest
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}