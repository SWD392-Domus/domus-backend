using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class PackageProductDetailRepository : GenericRepository<PackageProductDetail>, IPackageProductDetailRepository
{
    public PackageProductDetailRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}