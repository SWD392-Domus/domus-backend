using Domus.Api.Controllers.Base;
using Domus.Service.Constants;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;
using Domus.Service.Models.Requests.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;

[Authorize(Roles = UserRoleConstants.INTERNAL_USER, AuthenticationSchemes = "Bearer")]
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

	[HttpGet("{id}")]
	public async Task<IActionResult> GetUser(string id)
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

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateUser([FromForm] UpdateUserRequest request, string id)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.UpdateUser(request, id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteUser(string id)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.DeleteUser(id).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[AllowAnonymous]
	[HttpGet("self-profile/{token}")]
	public async Task<IActionResult> GetUserSelfProfile(string token)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.GetSelfProfile(token).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[AllowAnonymous]
	[HttpPut("self-profile/{token}")]
	public async Task<IActionResult> UpdateSelfProfile([FromForm] UpdateUserRequest request, string token)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.UpdateSelfProfile(request, token).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	[AllowAnonymous]
	[HttpPut("self-profile/{token}/password")]
	public async Task<IActionResult> UpdatePassword(UpdateUserPasswordRequest request, string token)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.UpdatePassword(request, token).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[Authorize(Roles = UserRoleConstants.ADMIN)]
	[HttpGet("staff")]
	public async Task<IActionResult> GetAllStaff()
	{
		return await ExecuteServiceLogic(
			async () => await _userService.GetAllStaff().ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	[HttpGet("search")]
	[Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
	public async Task<IActionResult> SearchUsersUsingGetRequest([FromQuery] SearchUsingGetRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.SearchUsersUsingGet(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	[HttpDelete("many")]
	[Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
	public async Task<IActionResult> DeleteMultipleUsers(List<string> userIds)
	{
		return await ExecuteServiceLogic(
			async () => await _userService.DeleteUsers(userIds).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	
	
}
