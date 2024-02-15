using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Services;

namespace Domus.Service.Interfaces;

public interface IServiceService : IAutoRegisterable
{
    Task<ServiceActionResult> GetAllServices();
    Task<ServiceActionResult> GetPaginatedServices(BasePaginatedRequest request);
    Task<ServiceActionResult> CreateService(CreateServiceRequest request);
    Task<ServiceActionResult> UpdateService(UpdateServiceRequest request, Guid serviceId);
    Task<ServiceActionResult> DeleteService(Guid serviceId);
    Task<ServiceActionResult> GetService(Guid serviceId);
    Task<bool> IsAllServicesExist(IEnumerable<Guid> serviceIds);
    Task<IQueryable<Domain.Entities.Service>> GetServices(IEnumerable<Guid> serviceId);
}

