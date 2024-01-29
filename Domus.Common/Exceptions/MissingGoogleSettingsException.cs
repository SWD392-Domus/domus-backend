namespace Domus.Common.Exceptions;

public class MissingGoogleSettingsException : ArgumentNullException
{
    public MissingGoogleSettingsException() : base("Missing google settings")
    {
    }
}