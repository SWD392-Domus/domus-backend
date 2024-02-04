namespace Domus.Domain.Dtos;

public class DtoDomusUser
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
	public string? ProfileImage { get; set; }
}
