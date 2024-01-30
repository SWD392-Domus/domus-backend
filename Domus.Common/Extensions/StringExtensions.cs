namespace Domus.Common.Extensions;

public static class StringExtensions
{
	public static bool ToBool(this string value)
	{
		return value.Trim().ToLower() == "true";
	}
}
