using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseApiController
{
	private readonly IAuthService _authService;
	private readonly IGoogleOAuthService _googleOAuthService;
	private readonly IFacebookOAuthService _facebookOAuthService;

    public AuthController(IAuthService authService, IGoogleOAuthService googleOAuthService, IFacebookOAuthService facebookOAuthService)
    {
        _authService = authService;
		_googleOAuthService = googleOAuthService;
		_facebookOAuthService = facebookOAuthService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _authService.RegisterAsync(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _authService.LoginAsync(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
  
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _authService.RefreshTokenAsync(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

	[HttpPost("google-oauth")]
	public async Task<IActionResult> GoogleLogin(GoogleLoginRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _googleOAuthService.LoginAsync(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}

	[HttpPost("facebook-oauth")]
	public async Task<IActionResult> FacebookLogin(FacebookLoginRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _facebookOAuthService.LoginAsync(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
	
	[HttpPost("confirm-otp")]
	public async Task<IActionResult> ConfirmOtp(ConfirmOtpRequest request)
	{
		return await ExecuteServiceLogic(
			async () => await _authService.ConfirmOtpAsync(request).ConfigureAwait(false)
		).ConfigureAwait(false);
	}
}
