using Domus.Domain.Entities;

namespace Domus.Service.Interfaces;

public interface IJwtService
{
    string GenerateToken(DomusUser user, IEnumerable<string> roles);
	Task<string> GenerateRefreshToken(string userId);
}
