using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class PackageNotFoundException : ArgumentNullException, INotFoundException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public PackageNotFoundException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public PackageNotFoundException()
    {
        _customMessage = "Package not found";
    }
}