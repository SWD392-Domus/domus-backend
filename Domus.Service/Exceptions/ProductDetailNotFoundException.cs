using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class ProductDetailNotFoundException : ArgumentNullException, INotFoundException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public ProductDetailNotFoundException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public ProductDetailNotFoundException()
    {
		_customMessage = "Product detail not found";
    }
}
