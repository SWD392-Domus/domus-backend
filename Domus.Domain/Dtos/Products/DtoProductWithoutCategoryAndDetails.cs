namespace Domus.Domain.Dtos.Products;

public class DtoProductWithoutCategoryAndDetails
{
	public Guid Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Brand { get; set; }

    public string? Description { get; set; }
}
