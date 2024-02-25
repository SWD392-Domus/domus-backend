namespace Domus.Common.Exceptions;

public class MissingFacebookSettingsException : ArgumentNullException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public MissingFacebookSettingsException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public MissingFacebookSettingsException()
    {
        _customMessage = "Missing facebook settings";
    }
}