using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseApiController
{
	private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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
}
