using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using AutoMapper.Execution;
using Domus.Common.Constants;
using Domus.Domain.Entities;

namespace Domus.Service.Models.Email;

public class OtpEmail : BaseEmail 
{
    [Required]
    public  string UserName { get; set; } = string.Empty;
    
    [Required]
    public  string Otp { get; set; } = string.Empty;
    
    public override string EmailBody => GenerateEmailBody();

    protected override string GenerateEmailBody()
    {
        return LoadEmailTemplate().Replace("{"+$"{nameof(OtpEmail.UserName)}"+"}", UserName)
            .Replace("{"+$"{nameof(OtpEmail.Otp)}"+"}", Otp);;
    }
    protected override string LoadEmailTemplate()
    {
        return File.ReadAllText(EmailTemplatePathConstants.OtpEmailPath);
    }
}