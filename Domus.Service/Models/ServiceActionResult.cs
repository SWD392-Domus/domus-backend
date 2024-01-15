namespace Domus.Service.Models;

public class ServiceActionResult
{
	public bool IsSuccess { get; set; }
	public object? Data { get; set; }
	public string ? Detail { get; set; }

	public ServiceActionResult(bool isSuccess)
	{
		IsSuccess = isSuccess;
	}

	public ServiceActionResult(bool isSuccess, string detail)
	{
		IsSuccess = isSuccess;
		Detail = detail;
	}

	public ServiceActionResult() : this(true)
	{
	}
}
