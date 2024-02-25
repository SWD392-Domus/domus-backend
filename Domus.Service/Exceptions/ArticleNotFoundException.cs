using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class ArticleNotFoundException : ArgumentNullException, INotFoundException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public ArticleNotFoundException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public ArticleNotFoundException()
    {
		_customMessage = "Article not found";
    }
}
