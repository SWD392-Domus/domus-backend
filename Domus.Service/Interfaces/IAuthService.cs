using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Authentication;

namespace Domus.Service.Interfaces;

public interface IAuthService : IAutoRegisterable
{
	Task<ServiceActionResult> RegisterAsync(RegisterRequest request);
	Task<ServiceActionResult> LoginAsync(LoginRequest request);
	Task<ServiceActionResult> RefreshTokenAsync(RefreshTokenRequest request);
	Task<ServiceActionResult> AssignRoleAsync(AssignRoleRequest request);
}
