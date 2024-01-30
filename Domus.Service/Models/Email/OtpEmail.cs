using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using AutoMapper.Execution;
using Domus.Common.Constants;
using Domus.Domain.Entities;

namespace Domus.Service.Models.Email;

public class OtpEmail : Email,IBaseTemplateEmail 
{
    [Required]
    public  string UserName { get; set; } = string.Empty;
    
    [Required]
    public  string Otp { get; set; } = string.Empty;
    
    public override string EmailBody => GenerateEmailBody();

    public string GenerateEmailBody()
    {
        return LoadEmailTemplate().Replace("{"+$"{nameof(OtpEmail.UserName)}"+"}", UserName)
            .Replace("{"+$"{nameof(OtpEmail.Otp)}"+"}", Otp);;
    }
    public string LoadEmailTemplate()
    {
        return File.ReadAllText(EmailTemplatePathConstants.OtpEmailPath);
    }
}