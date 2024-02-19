using System.Text.RegularExpressions;
using AutoMapper;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Constants;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Authentication;
using Domus.Service.Models.Responses;
using Microsoft.AspNetCore.Identity;

namespace Domus.Service.Implementations;

public class AuthService : IAuthService
{
	private readonly UserManager<DomusUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserTokenRepository _userTokenRepository;

	public AuthService(
		UserManager<DomusUser> userManager,
		RoleManager<IdentityRole> roleManager,
		IMapper mapper,
		IJwtService jwtService,
		IUserRepository userRepository,
		IUnitOfWork unitOfWork, 
		IUserTokenRepository userTokenRepository)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_mapper = mapper;
		_jwtService = jwtService;
		_userRepository = userRepository;
		_unitOfWork = unitOfWork;
		_userTokenRepository = userTokenRepository;
	}

    public async Task<ServiceActionResult> LoginAsync(LoginRequest request)
	{
		var user = await _userRepository.GetAsync(u => u.UserName!.ToLower() == request.Email.ToLower());
		if (user == null)
		{
			throw new UserNotFoundException($"User '{request.Email}' does not exist");
		}
        
		var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
		if (!validPassword)
		{
			throw new InvalidPasswordException("Invalid password");
		}

		var roles = await _userManager.GetRolesAsync(user);
		var response = new AuthResponse
		{
			Username = user.UserName ?? user.Email ?? string.Empty,
			Roles = roles,
			Token = new TokenResponse
			{
				AccessToken = _jwtService.GenerateAccessToken(user, roles),
				RefreshToken = await _jwtService.GenerateRefreshToken(user.Id),
				ExpiresAt = DateTimeOffset.Now.AddHours(1)
			}
		};

		return new ServiceActionResult(true) { Data = response };
    }

    public async Task<ServiceActionResult> RefreshTokenAsync(RefreshTokenRequest request)
    {
	    var refreshToken = await _userTokenRepository.GetAsync(t => t.Value == request.RefreshToken);
	    if (refreshToken is null)
		    throw new RefreshTokenNotFoundException("Refresh token does not exist");

	    var user = await _userRepository.GetAsync(u => u.Id == refreshToken.UserId);
	    if (user is null)
		    throw new UserNotFoundException($"User '{refreshToken.UserId}' does not exist");
	    
	    var roles = await _userManager.GetRolesAsync(user);
		var tokenResponse = new TokenResponse
		{
			AccessToken = _jwtService.GenerateAccessToken(user, roles),
			RefreshToken = request.RefreshToken,
			ExpiresAt = DateTimeOffset.Now.AddHours(1)
		};

		return new ServiceActionResult(true) { Data = tokenResponse };
    }

    public async Task<ServiceActionResult> AssignRoleAsync(AssignRoleRequest request)
    {
	    var user = await _userRepository.GetAsync(u => u.Email == request.Email);
	    if (user == null)
	    {
		    throw new UserNotFoundException($"User '{request.Email}' does not exist.");
	    }

	    await EnsureRoleExistsAsync(request.RoleName);

	    await _userManager.AddToRoleAsync(user, request.RoleName);
	    return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> RegisterAsync(RegisterRequest request)
    {
	    if (await _userRepository.ExistsAsync(u => u.Email!.ToLower() == request.Email.ToLower()))
		    throw new UserAlreadyExistsException($"User '{request.Email}' already exists");

	    if (!Regex.IsMatch(request.Password, PasswordConstants.PasswordPattern))
		    throw new PasswordTooWeakException(PasswordConstants.PasswordPatternErrorMessage);
	    
	    var user = _mapper.Map<DomusUser>(request);

	    var result = await _userManager.CreateAsync(user, request.Password);
	    await EnsureRoleExistsAsync(UserRoleConstants.CLIENT);
	    await _userManager.AddToRoleAsync(user, UserRoleConstants.CLIENT);
	    if (result.Succeeded)
	    {
		    var returnedUser = await _userRepository.GetAsync(u => u.Email == request.Email);
		    return new ServiceActionResult(true) { Data = _mapper.Map<DtoDomusUser>(returnedUser) };
	    }

	    var error = result.Errors.First();
	    return new ServiceActionResult(false, error.Description);
    }

    private async Task EnsureRoleExistsAsync(string role)
    {
	    if (!await _roleManager.RoleExistsAsync(role))
	    {
		    await _roleManager.CreateAsync(new IdentityRole(role));
	    }
    }
}
