namespace Domus.Common.Exceptions;

public class MissingVnpaySettingsException : ArgumentNullException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public MissingVnpaySettingsException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public MissingVnpaySettingsException()
    {
        _customMessage = "Missing vnpay settings";
    }
}
