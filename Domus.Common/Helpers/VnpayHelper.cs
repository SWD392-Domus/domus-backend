using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Domus.Common.Helpers;

public static class VnpayHelper
{
	public static string BuildPaymentUrl(string url, string secret, string queryString)
	{
		return $"{url}?{queryString}&vnp_SecureHash={VnpayHelper.HashText(secret, queryString)}";
	}

	public static string BuildQueryString(SortedList<string, string> requestData)
	{
		var data = new StringBuilder();
		foreach (KeyValuePair<string, string> kv in requestData)
		{
			if (!string.IsNullOrEmpty(kv.Value))
			{
				data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
			}
		}
		var queryString = data.ToString();
		if (queryString.Length > 0)
		{
			queryString= queryString.Remove(data.Length - 1, 1);
		}

		return queryString;
	}

	private static string HashText(string secret, string text)
	{
		var hash = new StringBuilder(); 
		byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
		byte[] inputBytes = Encoding.UTF8.GetBytes(text);
		using (var hmac = new HMACSHA512(keyBytes))
		{
			byte[] hashValue = hmac.ComputeHash(inputBytes);
			foreach (var theByte in hashValue)
			{
				hash.Append(theByte.ToString("x2"));
			}
		}

		return hash.ToString();
	}
}
