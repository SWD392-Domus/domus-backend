using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class ServiceNotFoundException : ArgumentNullException, INotFoundException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public ServiceNotFoundException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public ServiceNotFoundException()
    {
        _customMessage = "Service not found";
    }
}