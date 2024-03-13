namespace Domus.Service.Models.Requests.Dashboard;

public class GetDashboardInfoRequest
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}