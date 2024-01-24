using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Authentication;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}
