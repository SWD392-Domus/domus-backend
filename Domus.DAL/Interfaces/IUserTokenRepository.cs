using Domus.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domus.DAL.Interfaces;

public interface IUserTokenRepository : IGenericRepository<IdentityUserToken<string>>, IAutoRegisterable
{
}
