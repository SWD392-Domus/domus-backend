namespace Domus.Service.Models.Requests.Base;

public class BaseSearchRequest : BasePaginatedRequest
{
    public IEnumerable<SearchInfo> ConjunctionSearchInfos { get; set; } = new List<SearchInfo>();
    public IEnumerable<SearchInfo> DisjunctionSearchInfos { get; set; } = new List<SearchInfo>();
    public IEnumerable<SortInfo> SortInfos { get; set; } = new List<SortInfo>();
}