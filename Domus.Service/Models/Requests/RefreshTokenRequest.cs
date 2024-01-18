using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}