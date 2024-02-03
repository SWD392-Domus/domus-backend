namespace Domus.Common.Exceptions;

public class ProductCategoryNotFoundException : ArgumentNullException, INotFoundException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public ProductCategoryNotFoundException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public ProductCategoryNotFoundException()
    {
		_customMessage = "Product category not found";
    }
}
