using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Users;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;

[Route("api/[controller]")]
public class UsersController : BaseApiController
{
	private readonly IUserService _userService;

	public UsersController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpGet]
	public async Task<IActionResult> GetPaginatedUsers([FromQuery] BasePaginatedRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.GetPaginatedUsers(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpGet("all")]
	public async Task<IActionResult> GetAllUsers()
	{
		return await ExecuteServiceLogic(
			async () => await _userService.GetAllUsers().ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetUser(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.GetUser(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPost]
	public async Task<IActionResult> CreateUser(CreateUserRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.CreateUser(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateUser(UpdateUserRequest request, Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.UpdateUser(request, id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteUser(Guid id)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.DeleteUser(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
}
