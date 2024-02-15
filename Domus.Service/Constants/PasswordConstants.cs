namespace Domus.Service.Constants;

public static class PasswordConstants
{
	public const string PasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
	public const string PasswordPatternErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one number and one special character";
}
