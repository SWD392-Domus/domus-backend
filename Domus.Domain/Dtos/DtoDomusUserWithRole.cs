namespace Domus.Domain.Dtos;

public class DtoDomusUserWithRole : DtoDomusUser
{
    public IList<string>? Role { get; set; } = new List<string>();
}