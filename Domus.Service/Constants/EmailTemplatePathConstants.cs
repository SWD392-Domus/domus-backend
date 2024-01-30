namespace Domus.Common.Constants;
using System.IO;
using System;
public static class EmailTemplatePathConstants
{
    public static readonly string OtpEmailPath;
    
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory ?? throw new ArgumentNullException("App Domain Not Found");
    private const string RELATIVE_PATH_TO_OTP_EMAIL = @"EmailTemplates\OtpEmailTemplate.html";

    static EmailTemplatePathConstants()
    {
        OtpEmailPath = Path.Combine(BasePath, RELATIVE_PATH_TO_OTP_EMAIL);
    }
}