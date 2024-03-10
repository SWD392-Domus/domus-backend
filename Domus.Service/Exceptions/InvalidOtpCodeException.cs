using System.Security.Authentication;
using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class InvalidOtpCodeException : InvalidCredentialException, IBusinessException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public InvalidOtpCodeException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public InvalidOtpCodeException()
    {
        _customMessage = "Otp code is invalid";
    }
}
