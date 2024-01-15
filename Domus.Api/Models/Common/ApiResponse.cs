using Domus.Api.Enums;

namespace Domus.Api.Models.Common;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public IList<ApiMessage> Messages { get; set; }
    public ApiResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
        Messages = new List<ApiMessage>();
    }

    public ApiResponse() : this(false)
    {
    }

    public ApiResponse<T> AddMessage(string messageContent, ApiMessageType type)
    {
        Messages.Add(new ApiMessage() {Content = messageContent, Type = type});
        return this;
    }

    public ApiResponse<T> AddSuccessMessage(string messageContent)
    {
        Messages.Add(new ApiMessage(){ Content = messageContent, Type = ApiMessageType.Success});
        return this;
    }
    
    public ApiResponse<T> AddWarningMessage(string messageContent)
    {
        Messages.Add(new ApiMessage(){ Content = messageContent, Type = ApiMessageType.Warning});
        return this;
    }
    
    public ApiResponse<T> AddErrorMessage(string messageContent)
    {
        Messages.Add(new ApiMessage(){ Content = messageContent, Type = ApiMessageType.Error});
        return this;
    }
}

public class ApiResponse : ApiResponse<object>
{
    public ApiResponse(bool isSuccess): base(isSuccess)
    {
    }

    public ApiResponse() : this(false)
    {
    }

    public ApiResponse(object data) : this()
    {
        Data = data;
    }
}
