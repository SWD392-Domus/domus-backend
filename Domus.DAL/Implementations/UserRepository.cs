using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class UserRepository : GenericRepository<DomusUser>, IUserRepository
{
    public UserRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}