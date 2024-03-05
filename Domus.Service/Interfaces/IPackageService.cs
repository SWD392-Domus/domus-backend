using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.OfferedPackages;
using Domus.Service.Models.Requests.Products;

namespace Domus.Service.Interfaces;

public interface IPackageService : IAutoRegisterable
{
    Task<ServiceActionResult> GetAllPackages();
    Task<ServiceActionResult> GetPaginatedPackages(BasePaginatedRequest request);
    Task<ServiceActionResult> GetPackage(Guid packageId);
    Task<ServiceActionResult> CreatePackage(PackageRequest request);
    Task<ServiceActionResult> UpdatePackage(PackageRequest request, Guid packageId);
    Task<ServiceActionResult> DeletePackage(Guid packageId);
    Task<ServiceActionResult> GetPackageByName(string name);
    Task<ServiceActionResult> SearchPackages(BaseSearchRequest request);
    Task<ServiceActionResult> SearchPackagesUsingGet(SearchUsingGetRequest request);
    Task<ServiceActionResult> DeletePackages(List<Guid> packageIds);
}
