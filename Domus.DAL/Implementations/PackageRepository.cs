using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class PackageRepository : GenericRepository<Package>, IPackageRepository
{
    public PackageRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}