using Domus.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domus.DAL.Implementations;

public class UserTokenRepository : GenericRepository<IdentityUserToken<string>>, IUserTokenRepository
{
    public UserTokenRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}