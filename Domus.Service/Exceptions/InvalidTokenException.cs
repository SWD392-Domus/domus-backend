using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class InvalidTokenException : ArgumentException, IBusinessException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public InvalidTokenException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public InvalidTokenException()
    {
        _customMessage = "Provided token is invalid";
    }
}