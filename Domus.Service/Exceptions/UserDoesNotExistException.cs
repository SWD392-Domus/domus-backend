using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class UserDoesNotExistException : ArgumentNullException, IBusinessException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public UserDoesNotExistException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public UserDoesNotExistException()
    {
    }
}