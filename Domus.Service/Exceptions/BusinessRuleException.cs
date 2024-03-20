using System.Security.Authentication;
using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class BusinessRuleException : InvalidCredentialException, IBusinessException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public BusinessRuleException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public BusinessRuleException()
    {
        _customMessage = "Business rule is violated";
    }
}
