using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Dashboard;
using Domus.Service.Models.Responses;

namespace Domus.Service.Implementations;

public class AdminService : IAdminService
{
    public async Task<ServiceActionResult> GetDashboardInfo(GetDashboardInfoRequest request)
    {
        var dashboardResponse = new DashboardResponse();
        dashboardResponse.RevenueByMonths.Add(new()
        {
            Revenue = 1999.99f,
            MonthAsNumber = 1,
            MonthAsString = "January"
        });

        await Task.CompletedTask;
        return new ServiceActionResult(true) { Data = dashboardResponse };
    }
}