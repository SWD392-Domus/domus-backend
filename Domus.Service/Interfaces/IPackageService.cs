using Domus.Domain.Entities;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.OfferedPackages;

namespace Domus.Service.Interfaces;

public interface IPackageService
{
    Task<ServiceActionResult> GetAllPackages();
    Task<ServiceActionResult> GetPaginatedPackages(BasePaginatedRequest request);
    Task<ServiceActionResult> GetPackage(Guid packageId);
    Task<ServiceActionResult> CreatePackage(CreatePackageRequest request);
    Task<ServiceActionResult> UpdatePackage(CreatePackageRequest request);
    Task<ServiceActionResult> DeletePackage(Guid packageId);
}