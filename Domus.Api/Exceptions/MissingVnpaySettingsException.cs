namespace Domus.Common.Exceptions;

public class MissingVnpaySettingsException : ArgumentNullException
{
	public MissingVnpaySettingsException() : base("Missing Vnpay settings")
	{
	}
}
