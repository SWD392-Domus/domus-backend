using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Email;

public class Email
{
    [Required]
    public string To { get; set; } =  String.Empty;
    [Required]
    public string Subject { get; set; } = String.Empty;
    [Required]
    public string EmailBody { get; set; } = String.Empty;
}