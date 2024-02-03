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
}
