using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Authentication;

public class ConfirmOtpRequest
{
    [Required]
    public string Otp { get; set; } = null!;
}