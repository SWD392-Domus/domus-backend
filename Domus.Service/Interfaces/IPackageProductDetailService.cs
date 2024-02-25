using Domus.Common.Interfaces;
using Domus.Domain.Entities;

namespace Domus.Service.Interfaces;

public interface IPackageProductDetailService : IAutoRegisterable
{
    Task<IQueryable<PackageProductDetail>> GetByProductDetailIds(List<Guid> productDetailIds);
}