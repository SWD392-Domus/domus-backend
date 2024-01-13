namespace Domus.Common.Exceptions;

public class MissingConnectionStringException : ArgumentNullException
{
	public override string Message => CustomeMessage ?? Message;
	private string? CustomeMessage { get; set; }

	public MissingConnectionStringException(string customemessage)
	{
		CustomeMessage = customemessage;
	}

	public MissingConnectionStringException()
	{
	}
}
