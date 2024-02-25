using System.Globalization;

namespace Domus.Common.Models.Common;

public class VnpayParamComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
		if (x == y) return 0;
		if (x == null) return -1;
		if (y == null) return 1;
		var vnpCompare = CompareInfo.GetCompareInfo("en-US");
		return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
    }
}
