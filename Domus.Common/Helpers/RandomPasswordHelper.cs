namespace Domus.Common.Helpers;

public static class RandomPasswordHelper
{
	private static readonly Random _random = new Random();

	public static string GenerateRandomPassword(int length)
	{
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		const string sepecialChars = "!@#$%^&*()_+";
		return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[_random.Next(s.Length)])
			.ToArray()
			.Append(sepecialChars[_random.Next(sepecialChars.Length)]).ToArray());
	}
	public static string GenerateRandomDigitPassword(int length)
	{
		const string digits = "0123456789";
		return new string(Enumerable.Repeat(digits, length)
			.Select(s => s[_random.Next(s.Length)])
			.ToArray());
	}
}
