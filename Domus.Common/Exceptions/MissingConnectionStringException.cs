namespace Domus.Common.Exceptions;

public class MissingConnectionStringException : ArgumentNullException
{
	public override string Message => CustomMessage ?? Message;
	private string? CustomMessage { get; set; }

	public MissingConnectionStringException(string customMessage)
	{
		CustomMessage = customMessage;
	}

	public MissingConnectionStringException()
	{
	}
}
