using System.Linq.Expressions;
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
using Domus.Service.Models.Requests.Products;
using Domus.Service.Models.Requests.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domus.Service.Implementations;

public class UserService : IUserService
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly UserManager<DomusUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IJwtService _jwtService;
	private readonly IFileService _fileService;

	public UserService(
		IUserRepository userRepository,
		IMapper mapper,
		IUnitOfWork unitOfWork,
		UserManager<DomusUser> userManager,
		RoleManager<IdentityRole> roleManager, IJwtService jwtService, IFileService fileService)
	{
		_userRepository = userRepository;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_userManager = userManager;
		_roleManager = roleManager;
		_jwtService = jwtService;
		_fileService = fileService;
	}

    public async Task<ServiceActionResult> CreateUser(CreateUserRequest request)
    {
		if (await _userRepository.ExistsAsync(u => u.Email == request.Email))
			throw new UserAlreadyExistsException("The email is already in use");
		if (await _userRepository.ExistsAsync(u => u.UserName == request.UserName))
			throw new UserAlreadyExistsException("The username is already in use");
		if (!Regex.IsMatch(request.Password, PasswordConstants.PasswordPattern))
			throw new PasswordTooWeakException(PasswordConstants.PasswordPatternErrorMessage);
		if (!string.IsNullOrEmpty(request.Role) && !await _roleManager.RoleExistsAsync(request.Role.ToUpper()))
			throw new RoleNotFoundException();

	    var user = _mapper.Map<DomusUser>(request);

	    var result = await _userManager.CreateAsync(user, request.Password);
		if (string.IsNullOrEmpty(request.Role))
		{
			await EnsureRoleExistsAsync(UserRoleConstants.CLIENT);
			await _userManager.AddToRoleAsync(user, UserRoleConstants.CLIENT);
		}
		else if (request.Role == UserRoleConstants.STAFF)
		{
			await EnsureRoleExistsAsync(UserRoleConstants.CLIENT);
			await EnsureRoleExistsAsync(UserRoleConstants.STAFF);
			await _userManager.AddToRoleAsync(user, UserRoleConstants.CLIENT);
			await _userManager.AddToRoleAsync(user, UserRoleConstants.STAFF);
		}
		else 
		{
			await EnsureRoleExistsAsync(request.Role.ToUpper());
			await _userManager.AddToRoleAsync(user, request.Role.ToUpper());
		}

	    if (result.Succeeded)
	    {
		    var returnedUser = await _userRepository.GetAsync(u => u.Email == request.Email);
		    return new ServiceActionResult(true) { Detail = "User created successfully" };
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

    public async Task<ServiceActionResult> GetSelfProfile(string token)
    {
	    if (!_jwtService.IsValidToken(token))
		    throw new InvalidTokenException();

	    var userId = _jwtService.GetTokenClaim(token, TokenClaimConstants.SUBJECT)?.ToString() ?? throw new UserNotFoundException();
		var user = await _userRepository.GetAsync(u => u.Id == userId && !u.IsDeleted) ?? throw new UserNotFoundException();
		var userRoles = await _userManager.GetRolesAsync(user);
		var userDto = _mapper.Map<DtoDomusUserWithRole>(user);
		userDto.Role = userRoles;

	    return new ServiceActionResult(true) { Data = userDto };
    }

    public async Task<ServiceActionResult> GetAllUsers()
    {
	    // var clientUser = await _userRepository.GetAsync(x => x.Id.Equals(request.ClientId)&& !x.IsDeleted) ??
	    //                  throw new Exception($"Not found Client: {request.ClientId}");
	    // var clientRoles = await _userManager.GetRolesAsync(clientUser);
	    var users = await (await _userRepository.FindAsync(u => !u.IsDeleted)).ToListAsync();
	    var dtoList = new List<DtoDomusUserWithRole>();
		foreach (var user in users)
		{
			var userRole = await _userManager.GetRolesAsync(user);
			var x = _mapper.Map<DtoDomusUserWithRole>(user);
			x.Role = userRole;
			dtoList.Add(x);
		}

		return new ServiceActionResult(true) { Data = dtoList };
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
		var userDto = _mapper.Map<DtoDomusUserWithRole>(user);
		var userRols = await _userManager.GetRolesAsync(user);
		userDto.Role = userRols;

		return new ServiceActionResult(true) { Data = userDto };
    }

    public async Task<ServiceActionResult> UpdateUser(UpdateUserRequest request, string userId)
    {
		var user = await _userRepository.GetAsync(u => u.Id == userId && !u.IsDeleted) ?? throw new UserNotFoundException();

		if (!string.IsNullOrEmpty(request.Email) && await _userRepository.ExistsAsync(u => u.Email == request.Email && u.Id != userId))
			throw new UserAlreadyExistsException("The email is already in use");
		if (!string.IsNullOrEmpty(request.UserName) && await _userRepository.ExistsAsync(u => u.UserName == request.UserName && u.Id != userId))
			throw new UserAlreadyExistsException("The username is already in use");

		_mapper.Map(request, user);
		
		if (request.ProfileImage != null)
		{
			var profileImageUrl = await _fileService.GetUrlAfterUploadedFile(new List<IFormFile> { request.ProfileImage }!);
			user.ProfileImage = profileImageUrl.First();
		}

		await _userRepository.UpdateAsync(user);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true) { Detail = "User updated successfully" };
    }

    private async Task EnsureRoleExistsAsync(string role)
    {
	    if (!await _roleManager.RoleExistsAsync(role))
	    {
		    await _roleManager.CreateAsync(new IdentityRole(role));
	    }
    }

    public async Task<ServiceActionResult> UpdateSelfProfile(UpdateUserRequest request, string token)
    {
	    if (!_jwtService.IsValidToken(token))
		    throw new InvalidTokenException();

	    var userId = _jwtService.GetTokenClaim(token, TokenClaimConstants.SUBJECT)?.ToString() ?? throw new UserNotFoundException();
	    var user = await _userManager.Users.Where(u => u.Id == userId)
		    .FirstOrDefaultAsync() ?? throw new UserNotFoundException();

		if (!string.IsNullOrEmpty(request.Email) && await _userRepository.ExistsAsync(u => u.Email == request.Email && u.Id != userId))
			throw new UserAlreadyExistsException("The email is already in use");
		if (!string.IsNullOrEmpty(request.UserName) && await _userRepository.ExistsAsync(u => u.UserName == request.UserName && u.Id != userId))
			throw new UserAlreadyExistsException("The username is already in use");

		_mapper.Map(request, user);
		
		if (request.ProfileImage != null)
		{
			var profileImageUrl = await _fileService.GetUrlAfterUploadedFile(new List<IFormFile> { request.ProfileImage }!);
			user.ProfileImage = profileImageUrl.First();
		}
		
		await _userManager.UpdateAsync(user);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true) { Detail = "User profile updated successfully" };
    }

    public async Task<ServiceActionResult> UpdatePassword(UpdateUserPasswordRequest request, string token)
    {
	    if (!_jwtService.IsValidToken(token))
		    throw new InvalidTokenException();
	    if (!Regex.IsMatch(request.NewPassword, PasswordConstants.PasswordPattern))
		    throw new PasswordTooWeakException(PasswordConstants.PasswordPatternErrorMessage);
	    if (request.NewPassword == request.CurrentPassword)
		    throw new InvalidPasswordException("New password can not be the same as the current one");

	    var userId = _jwtService.GetTokenClaim(token, TokenClaimConstants.SUBJECT)?.ToString() ?? throw new UserNotFoundException();
	    var user = await _userManager.Users.Where(u => u.Id == userId)
		    .FirstOrDefaultAsync() ?? throw new UserNotFoundException();
	    
	    if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
		    throw new InvalidPasswordException("Invalid password");
	    
	    var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
	    
	    return new ServiceActionResult(result.Succeeded)
	    {
			Detail = result.Succeeded
				? "Password updated successfully"
				: result.Errors.First().Description
	    };
    }

    public async Task<ServiceActionResult> GetAllStaff()
    {
	    var users = (await (await _userRepository.FindAsync(u => !u.IsDeleted && u.EmailConfirmed))
		    .ToListAsync())
		    .Where(u =>
		    {
			    var roles = _userManager.GetRolesAsync(u).Result;
			    return roles.Contains(UserRoleConstants.STAFF) &&
			           !roles.Contains(UserRoleConstants.ADMIN);
		    });

		return new ServiceActionResult(true) { Data = _mapper.Map<IEnumerable<DtoDomusUser>>(users) };
    }

    public async Task<ServiceActionResult> SearchUsersUsingGet(SearchUsingGetRequest request)
    {
	    var users = await (await _userRepository.FindAsync(p => !p.IsDeleted))
		    .ProjectTo<DtoDomusUser>(_mapper.ConfigurationProvider)
		    .ToListAsync();
      
        
	    if (!string.IsNullOrEmpty(request.SearchField))
	    {
		    users = users
			    .Where(p => ReflectionHelper.GetStringValueByName(typeof(DtoDomusUser), request.SearchField, p).Contains(request.SearchValue ?? string.Empty, StringComparison.OrdinalIgnoreCase))
			    .ToList();
	    }
	    if (!string.IsNullOrEmpty(request.SortField))
	    {
		    Expression<Func<DtoDomusUser, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(DtoDomusUser), request.SortField, p);
		    users = request.Descending
			    ? users.OrderByDescending(orderExpr.Compile()).ToList()
			    : users.OrderBy(orderExpr.Compile()).ToList();
	    }

	    var paginatedResult = PaginationHelper.BuildPaginatedResult(users, request.PageSize, request.PageIndex);
	    var finalUsers = (IEnumerable<DtoDomusUser>)paginatedResult.Items!;

	    paginatedResult.Items = finalUsers;

	    return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> DeleteUsers(List<string> userIds)
    {
	    var users = new List<DomusUser>();
	    foreach (var userId in userIds)
	    {
		    var user = await _userRepository.GetAsync(y => y.Id.Equals(userId)) ?? throw new UserNotFoundException($"Not found user: {userId}");
		    user.IsDeleted = true;
		    users.Add(user); 
	    }
	    await _userRepository.UpdateManyAsync(users);
	    await _unitOfWork.CommitAsync();
	    return new ServiceActionResult(true);
    }
}
