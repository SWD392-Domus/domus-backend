namespace Domus.Common.Constants;
using System.IO;
using System;
public static class EmailTemplatePathConstants
{
    public static readonly string OtpEmailPath;
    public static readonly string PasswordEmailPath;
    public static readonly string ContractEmailPath;
    public static readonly string SignedContractEmailPath;
    
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory ?? throw new ArgumentNullException("App Domain Not Found");
    private const string RELATIVE_PATH_TO_OTP_EMAIL = @"EmailTemplates/OtpEmailTemplate.html";
    private const string RELATIVE_PATH_TO_PASSWORD_EMAIL = @"EmailTemplates/PasswordEmailTemplate.html";
    private const string RELATIVE_PATH_TO_CONTRACT_EMAIL = @"EmailTemplates/ContractEmailTemplate.html";
    private const string RELATIVE_PATH_TO_SIGNED_CONTRACT_EMAIL = @"EmailTemplates/SignedContractEmailTemplate.html";
    static EmailTemplatePathConstants()
    {
        OtpEmailPath = Path.Combine(BasePath, RELATIVE_PATH_TO_OTP_EMAIL);
        PasswordEmailPath = Path.Combine(BasePath, RELATIVE_PATH_TO_PASSWORD_EMAIL);
        ContractEmailPath = Path.Combine(BasePath, RELATIVE_PATH_TO_CONTRACT_EMAIL);
        SignedContractEmailPath = Path.Combine(BasePath, RELATIVE_PATH_TO_SIGNED_CONTRACT_EMAIL);
    }
}