using Domus.DAL.Interfaces;
using Domus.Domain.Entities;
using Domus.Service.Interfaces;

namespace Domus.Service.Implementations;

public class PackageProductDetailService : IPackageProductDetailService
{
    private readonly IPackageProductDetailRepository _packageProductDetailRepository;
    public PackageProductDetailService(IPackageProductDetailRepository packageProductDetailRepository)
    {
        _packageProductDetailRepository = packageProductDetailRepository;
    }
    public Task<IQueryable<PackageProductDetail>> GetByProductDetailIds(List<Guid> productDetailIds)
    {
       return _packageProductDetailRepository.FindAsync(x => productDetailIds.Contains(x.ProductDetailId));
    }
}