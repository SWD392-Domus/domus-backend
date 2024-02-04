using System.Text.RegularExpressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Constants;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Users;
using Microsoft.AspNetCore.Identity;

namespace Domus.Service.Implementations;

public class UserService : IUserService
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly UserManager<DomusUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public UserService(
		IUserRepository userRepository,
		IMapper mapper,
		IUnitOfWork unitOfWork,
		UserManager<DomusUser> userManager,
		RoleManager<IdentityRole> roleManager
	)
	{
		_userRepository = userRepository;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_userManager = userManager;
		_roleManager = roleManager;
	}

    public async Task<ServiceActionResult> CreateUser(CreateUserRequest request)
    {
		if (await _userRepository.ExistsAsync(u => u.Email == request.Email))
			throw new UserAlreadyExistsException("The email is already in use");
		if (await _userRepository.ExistsAsync(u => u.UserName == request.UserName))
			throw new UserAlreadyExistsException("The username is already in use");
		if (!Regex.IsMatch(request.Password, PasswordConstants.PasswordPattern))
			throw new PasswordTooWeakException(PasswordConstants.PasswordPatternErrorMessage);

	    var user = _mapper.Map<DomusUser>(request);

	    var result = await _userManager.CreateAsync(user, request.Password);
	    await EnsureRoleExistsAsync(UserRoleConstants.CLIENT);
	    await _userManager.AddToRoleAsync(user, UserRoleConstants.CLIENT);
	    if (result.Succeeded)
	    {
		    var returnedUser = await _userRepository.GetAsync(u => u.Email == request.Email);
		    return new ServiceActionResult(true) { Detail = "User created succeessfully" };
	    }

	    var error = result.Errors.First();
	    return new ServiceActionResult(false, error.Description);
    }

    public async Task<ServiceActionResult> DeleteUser(string userId)
    {
		var user = await _userRepository.GetAsync(u => u.Id == userId && !u.IsDeleted) ?? throw new UserNotFoundException();
		user.IsDeleted = true;

		await _userRepository.UpdateAsync(user);
		await _unitOfWork.CommitAsync();
		return new ServiceActionResult(true) { Detail = "User deleted successfully" };
    }

    public async Task<ServiceActionResult> GetAllUsers()
    {
		var users = (await _userRepository.GetAllAsync())
			.Where(u => !u.IsDeleted)
			.ProjectTo<DtoDomusUser>(_mapper.ConfigurationProvider);

		return new ServiceActionResult(true) { Data = users };
    }

    public async Task<ServiceActionResult> GetPaginatedUsers(BasePaginatedRequest request)
    {
		var queryableUsers = (await _userRepository.GetAllAsync())
			.Where(u => !u.IsDeleted)
			.ProjectTo<DtoDomusUser>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(queryableUsers, request.PageSize, request.PageIndex);

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> GetUser(string userId)
    {
		var user = await _userRepository.GetAsync(u => u.Id == userId && !u.IsDeleted) ?? throw new UserNotFoundException();

		return new ServiceActionResult(true) { Data = _mapper.Map<DtoDomusUser>(user) };
    }

    public Task<ServiceActionResult> UpdateUser(UpdateUserRequest request, string userId)
    {
        throw new NotImplementedException();
    }

    private async Task EnsureRoleExistsAsync(string role)
    {
	    if (!await _roleManager.RoleExistsAsync(role))
	    {
		    await _roleManager.CreateAsync(new IdentityRole(role));
	    }
    }
}
