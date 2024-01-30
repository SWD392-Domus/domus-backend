using System.ComponentModel.DataAnnotations;
using Domus.Common.Constants;

namespace Domus.Service.Models.Email;

public class PasswordEmail : BaseEmail
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    public override string EmailBody => GenerateEmailBody();
    protected override string GenerateEmailBody()
    {
        return LoadEmailTemplate().Replace("{"+$"{nameof(PasswordEmail.UserName)}"+"}", UserName)
            .Replace("{"+$"{nameof(PasswordEmail.Password)}"+"}", Password);;
    }

    protected override string LoadEmailTemplate()
    {
        return File.ReadAllText(EmailTemplatePathConstants.PasswordEmailPath);
    }
}