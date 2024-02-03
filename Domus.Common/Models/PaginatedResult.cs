namespace Domus.Common.Models;

public class PaginatedResult
{
	public int PageIndex { get; set; }
	public int PageSize { get; set; }
	public long LastPage { get; set; }
	public bool IsLastPage { get; set; }
	public long Total { get; set; }
	public object? Items { get; set; }
}
