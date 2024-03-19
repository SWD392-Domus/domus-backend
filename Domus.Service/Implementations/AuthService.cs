using System.Text.RegularExpressions;
using AutoMapper;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Constants;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Email;
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
    private readonly IEmailService _emailService;
    private readonly IOtpRepository _otpRepository;

	public AuthService(
		UserManager<DomusUser> userManager,
		RoleManager<IdentityRole> roleManager,
		IMapper mapper,
		IJwtService jwtService,
		IUserRepository userRepository,
		IUnitOfWork unitOfWork, 
		IUserTokenRepository userTokenRepository, IEmailService emailService, IOtpRepository otpRepository)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_mapper = mapper;
		_jwtService = jwtService;
		_userRepository = userRepository;
		_unitOfWork = unitOfWork;
		_userTokenRepository = userTokenRepository;
		_emailService = emailService;
		_otpRepository = otpRepository;
	}

    public async Task<ServiceActionResult> LoginAsync(LoginRequest request)
	{
		var user = await _userRepository.GetAsync(u => u.UserName!.ToLower() == request.Email.ToLower() && u.EmailConfirmed);
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

	    var returnedUser = _mapper.Map<DtoDomusUser>(user);
		return new ServiceActionResult(true) { Data = new { userInfo = returnedUser, token = response } };
    }

    public async Task<ServiceActionResult> RefreshTokenAsync(RefreshTokenRequest request)
    {
	    var refreshToken = await _userTokenRepository.GetAsync(t => t.Value == request.RefreshToken);
	    if (refreshToken is null)
		    throw new RefreshTokenNotFoundException("Refresh token does not exist");

	    var user = await _userRepository.GetAsync(u => u.Id == refreshToken.UserId);
	    if (user is null)
		    throw new UserNotFoundException($"User '{refreshToken.UserId}' does not exist");

	    var tokenResponse = GenerateAuthResponseAsync(user);

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

    public async Task<ServiceActionResult> ConfirmOtpAsync(ConfirmOtpRequest request)
    {
	    var otp = await _otpRepository.GetAsync(o => o.Code == request.Otp && !o.Used && o.UserId == request.Id) ?? throw new InvalidOtpCodeException();
	    var user = await _userRepository.GetAsync(u => !u.IsDeleted && u.Id == otp.UserId && !u.EmailConfirmed) ??
	               throw new UserNotFoundException();
	    
	    otp.Used = true;
	    user.EmailConfirmed = true;
	    
	    await _otpRepository.UpdateAsync(otp);
	    await _userRepository.UpdateAsync(user);
	    await _unitOfWork.CommitAsync();
	    
	    var returnedUser = _mapper.Map<DtoDomusUser>(user);
	    var tokenResponse = await GenerateAuthResponseAsync(user);
	    return new ServiceActionResult(true) { Data = new { userInfo = returnedUser, token = tokenResponse } };
    }

    public async Task<ServiceActionResult> RegisterAsync(RegisterRequest request)
    {
	    var retrievedUser =
		    await _userRepository.GetAsync(u => u.Email!.ToLower() == request.Email.ToLower());
	    if (retrievedUser is { EmailConfirmed: true })
		    throw new UserAlreadyExistsException($"User '{request.Email}' already exists");

	    if (!Regex.IsMatch(request.Password, PasswordConstants.PasswordPattern))
		    throw new PasswordTooWeakException(PasswordConstants.PasswordPatternErrorMessage);

	    if (retrievedUser == null)
	    {
			retrievedUser = _mapper.Map<DomusUser>(request);
			var result = await _userManager.CreateAsync(retrievedUser, request.Password);
			await EnsureRoleExistsAsync(UserRoleConstants.CLIENT);
			await _userManager.AddToRoleAsync(retrievedUser, UserRoleConstants.CLIENT);
			
			if (!result.Succeeded)
			{
				var error = result.Errors.First();
				return new ServiceActionResult(false, error.Description);
			}
	    }
	    
		var otp = new Otp
		{
			UserId = retrievedUser.Id,
			Used = false,
			CreatedAt= DateTime.Now.AddHours(7),
			Code = RandomPasswordHelper.GenerateRandomPassword(10)
		};
		await _otpRepository.AddAsync(otp);
		await _unitOfWork.CommitAsync();
		
		_emailService.SendOtpEmail(new OtpEmail
		{
			UserName = retrievedUser.UserName!,
			Subject = "Email confirmation",
			To = retrievedUser.Email!,
			Otp = otp.Code
		});

		return new ServiceActionResult(true) { Data = new { Id = otp.UserId } };
    }

    private async Task EnsureRoleExistsAsync(string role)
    {
	    if (!await _roleManager.RoleExistsAsync(role))
	    {
		    await _roleManager.CreateAsync(new IdentityRole(role));
	    }
    }

    private async Task<AuthResponse> GenerateAuthResponseAsync(DomusUser user)
    {
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

		return response;
    }
}
