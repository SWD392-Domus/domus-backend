using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class ProductNotFoundException : ArgumentNullException, INotFoundException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public ProductNotFoundException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public ProductNotFoundException()
    {
		_customMessage = "Product not found";
    }
}
