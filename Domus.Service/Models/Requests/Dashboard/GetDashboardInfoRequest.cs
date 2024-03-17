using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Dashboard;

public class GetDashboardInfoRequest
{
    [Range(typeof(int),"1970", "2030")]
    public int Year { get; set; }
}