using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class OtpRepository : GenericRepository<Otp>, IOtpRepository
{
    public OtpRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}