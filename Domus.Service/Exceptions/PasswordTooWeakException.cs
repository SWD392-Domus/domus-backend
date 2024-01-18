using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class PasswordTooWeakException : ArgumentException, IBusinessException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public PasswordTooWeakException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public PasswordTooWeakException()
    {
    }
}