namespace Domus.Service.Exceptions;

public class UserDoesNotExistException : ArgumentNullException
{
    private string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public UserDoesNotExistException(string customMessage) : base(customMessage)
    {
    }
    
    public UserDoesNotExistException()
    {
    }
}