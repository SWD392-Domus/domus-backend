using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Dashboard;

namespace Domus.Service.Interfaces;

public interface IAdminService : IAutoRegisterable
{
    Task<ServiceActionResult> GetDashboardInfo(GetDashboardInfoRequest request);
}