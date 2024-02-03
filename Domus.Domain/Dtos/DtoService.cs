namespace Domus.Domain.Dtos;

public class DtoService
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public string MonetaryUnit { get; set; } = null!;

}