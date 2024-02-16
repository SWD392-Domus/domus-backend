namespace Domus.Common.Exceptions;

public class MissingGoogleSettingsException : ArgumentNullException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public MissingGoogleSettingsException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public MissingGoogleSettingsException()
    {
        _customMessage = "Missing google settings";
    }
}