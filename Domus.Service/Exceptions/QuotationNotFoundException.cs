using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class QuotationNotFoundException : ArgumentNullException, INotFoundException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public QuotationNotFoundException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public QuotationNotFoundException()
    {
		_customMessage = "Quotation not found";
    }
}
