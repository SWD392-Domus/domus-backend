using System.ComponentModel.DataAnnotations;
using Domus.Common.Constants;

namespace Domus.Service.Models.Email;

public class SignedContractEmail : BaseEmail
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string ContractName { get; set; } = string.Empty;
    [Required] public string ContractorName { get; set; } = string.Empty;
    
    public override string EmailBody => GenerateEmailBody();
    protected override string GenerateEmailBody()
    {
        return LoadEmailTemplate().Replace($"{{{nameof(ContractEmail.UserName)}}}", UserName)
                .Replace($"{{{nameof(ContractEmail.ContractName)}}}", ContractName) 
                .Replace($"{{{nameof(ContractEmail.ContractorName)}}}", ContractorName)
            ;
    }

    protected override string LoadEmailTemplate()
    {
        return File.ReadAllText(EmailTemplatePathConstants.SignedContractEmailPath);
    }
}