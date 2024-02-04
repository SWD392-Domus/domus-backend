namespace Domus.Service.Constants;

public static class UserRoleConstants
{
	public const string ADMIN = "Admin";
    // public const string MANAGER = "Manager";
	public const string STAFF = "Staff";
	public const string INTERNAL_USER = ADMIN + "," + STAFF;
    public const string CLIENT = "Client";
}
