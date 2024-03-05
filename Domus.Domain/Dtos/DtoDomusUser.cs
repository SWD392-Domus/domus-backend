namespace Domus.Domain.Dtos;

public class DtoDomusUser
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
	public string UserName { get; set; } = null!;
    public string FullName { get; set; } = null!;
	public string Gender { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
	public string? ProfileImage { get; set; }
}
