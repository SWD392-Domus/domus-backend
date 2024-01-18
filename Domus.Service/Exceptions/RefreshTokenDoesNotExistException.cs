using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class RefreshTokenDoesNotExistException : ArgumentNullException, IBusinessException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public RefreshTokenDoesNotExistException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public RefreshTokenDoesNotExistException()
    {
    }
}