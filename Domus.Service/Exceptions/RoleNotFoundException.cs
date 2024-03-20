using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class RoleNotFoundException : ArgumentException, INotFoundException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public RoleNotFoundException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public RoleNotFoundException()
    {
        _customMessage = "Role not found";
    }
}
