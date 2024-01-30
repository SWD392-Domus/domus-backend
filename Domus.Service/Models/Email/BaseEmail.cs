using System.ComponentModel.DataAnnotations;
using static System.String;

namespace Domus.Service.Models.Email;

public abstract class BaseEmail
{
    [Required]
    public string To { get; set; } =  Empty;
    [Required]
    public string Subject { get; set; } = Empty;
    public virtual string EmailBody { get; set; } = Empty;
    protected abstract string GenerateEmailBody();
    protected abstract string LoadEmailTemplate();
}