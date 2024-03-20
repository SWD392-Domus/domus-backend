namespace Domus.Service.Models.Responses;

public class DashboardResponse
{
    public float TotalRevenue { get; set; }
    public int NewUsersCount { get; set; }
    public int QuotationsCount { get; set; }
    public int ContractsCount { get; set; }
    public IList<RevenueByMonth> RevenueByMonths { get; set; } = new List<RevenueByMonth>();
}

public class RevenueByMonth
{
    public string MonthAsString { get; set; }
    public int MonthAsNumber { get; set; }
    public float Revenue { get; set; }
}
