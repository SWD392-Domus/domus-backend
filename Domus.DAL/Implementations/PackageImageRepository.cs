using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class PackageImageRepository : GenericRepository<PackageImage>, IPackageImageRepository
{
    public PackageImageRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}