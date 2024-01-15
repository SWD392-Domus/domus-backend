using Domus.Api.Enums;

namespace Domus.Api.Models.Common;

public class ApiMessage
{
	public string Content { get; set; }
	public ApiMessageType Type { get; set; }

	public ApiMessage()
	{
		Content = string.Empty;
	}

	public ApiMessage(string content, ApiMessageType messageType = ApiMessageType.Info)
	{
		Content = content;
		Type = messageType;
	}
}
