using System.Security.Authentication;
using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class InvalidPasswordException : InvalidCredentialException, IBusinessException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public InvalidPasswordException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public InvalidPasswordException()
    {
    }
}