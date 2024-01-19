using Domus.Common.Exceptions;

namespace Domus.Service.Exceptions;

public class RefreshTokenNotFoundException : ArgumentNullException, INotFoundException 
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public RefreshTokenNotFoundException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public RefreshTokenNotFoundException()
    {
		_customMessage = "Refresh token not found";
    }
}
