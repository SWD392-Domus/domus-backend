using Domus.Service.Models;
using Domus.Service.Models.Requests.Authentication;

namespace Domus.Service.Interfaces;

public interface IOAuthService
{
	Task<ServiceActionResult> LoginAsync(OAuthRequest request);
}
