using Domus.Api.Constants;
using Domus.Common.Exceptions;
using Domus.Common.Helpers;
using Domus.Service.Models;
using Domus.Service.Models.Common;
using Microsoft.AspNetCore.Mvc;
using NLog;
using ILogger = NLog.ILogger;

namespace Domus.Api.Controllers.Base;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
	private readonly ILogger logger = LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName);
	private IActionResult BuildSuccessResult(ServiceActionResult result)
	{
		var successResult = new ApiResponse(true)
		{
			Data = result.Data,
			StatusCode = StatusCodes.Status200OK
		};

		var detail = result.Detail ?? ApiMessageConstants.SUCCESS;
		successResult.AddSuccessMessage(detail);
		return base.Ok(successResult);
	}
	
	private IActionResult BuildErrorResult(Exception ex)
	{
		var errorResult = new ApiResponse(false);
		errorResult.AddErrorMessage(ex.Message);

		var statusCode = 500;
		if (ex.GetType().IsAssignableTo(typeof(INotFoundException)))
			statusCode = StatusCodes.Status404NotFound;
		else if (ex.GetType().IsAssignableTo(typeof(IBusinessException)))
			statusCode = StatusCodes.Status409Conflict;

		errorResult.StatusCode = statusCode;
		return base.Ok(errorResult);
	}

	protected async Task<IActionResult> ExecuteServiceLogic(Func<Task<ServiceActionResult>> serviceLogicFunc)
	{
		return await ExecuteServiceLogic(serviceLogicFunc, null);

	}
	protected async Task<IActionResult> ExecuteServiceLogic(Func<Task<ServiceActionResult>> serviceLogicFunc, Func<Task<ServiceActionResult>>? errorHandler)
	{
		var startTime = DateTime.Now;
		StringInterpolationHelper.AppendToStart(serviceLogicFunc.Method.Name!);
		var methodInfo = StringInterpolationHelper.BuildAndClear();
		logger.Info($"[START] [API-Method] - {methodInfo}");

		try
		{
			var result = await serviceLogicFunc();

			StringInterpolationHelper.AppendToStart("Result of [[");
			StringInterpolationHelper.Append(methodInfo);
			StringInterpolationHelper.Append($"]]. IsSuccess: {result.IsSuccess}");
			StringInterpolationHelper.Append(". Detail: ");
			StringInterpolationHelper.Append(result.Detail ?? "no details.");
			logger.Info(StringInterpolationHelper.BuildAndClear());

			return result.IsSuccess ? BuildSuccessResult(result) : Problem(result.Detail);
		}
		catch (Exception ex)
		{
			if (errorHandler is not null)
				await errorHandler();

			StringInterpolationHelper.AppendToStart("Result of [[");
			StringInterpolationHelper.Append(methodInfo);
			StringInterpolationHelper.Append($"]]. IsSuccess: false");
			StringInterpolationHelper.Append(". Detail: ");
			StringInterpolationHelper.Append(ex.Message);
			logger.Info(StringInterpolationHelper.BuildAndClear());

			return BuildErrorResult(ex);
		}
		finally
		{
			StringInterpolationHelper.AppendToStart($"[END] - {methodInfo}. ");
			StringInterpolationHelper.Append("Total: ");
			StringInterpolationHelper.Append((DateTime.Now - startTime).Milliseconds.ToString());
			StringInterpolationHelper.Append(" ms.");
			logger.Info(StringInterpolationHelper.BuildAndClear());
		}
	}
}
