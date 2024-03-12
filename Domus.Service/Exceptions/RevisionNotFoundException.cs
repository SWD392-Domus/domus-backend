using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class RevisionNotFoundException : ArgumentNullException, INotFoundException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public RevisionNotFoundException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public RevisionNotFoundException()
    {
        _customMessage = "Revision not found";
    }
}