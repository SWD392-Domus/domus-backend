using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class ArticleCategoryNotFoundException : ArgumentNullException, IBusinessException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public ArticleCategoryNotFoundException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public ArticleCategoryNotFoundException()
    {
		_customMessage = "Article category not found";
    }
}
