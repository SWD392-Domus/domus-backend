using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class UserAlreadyExistsException : ArgumentException, IBusinessException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public UserAlreadyExistsException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public UserAlreadyExistsException()
    {
		_customMessage = "User already exists";
    }
}
