using Domus.Api.Enums;
using Domus.Api.Models.Common;
using NLog;
using ILogger = NLog.ILogger;

namespace Domus.Api.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger logger = LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName);

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = new ApiResponse(false);
            response.Messages.Add(new ApiMessage(ex.Message, ApiMessageType.Error));

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
