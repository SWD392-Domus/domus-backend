using Domus.Service.Models.Requests.Base;

namespace Domus.Service.Models.Requests.Products;

public class SearchProductsUsingGetRequest : BasePaginatedRequest
{
    public string? SearchField { get; set; }
    public string? SearchValue { get; set; }
    public string? SortField { get; set; }
    public bool Descending { get; set; }
}