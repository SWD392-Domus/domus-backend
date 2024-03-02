using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class UnauthorizedAccessException : ArgumentNullException,IForbiddenException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public UnauthorizedAccessException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public UnauthorizedAccessException()
    {
        _customMessage = "Unauthorized access";
    }
}