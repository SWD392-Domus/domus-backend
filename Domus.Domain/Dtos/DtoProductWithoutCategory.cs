namespace Domus.Domain.Dtos;

public class DtoProductWithoutCategory
{
	public Guid Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Brand { get; set; }

    public string? Description { get; set; }

    public ICollection<DtoProductDetail> ProductDetails { get; set; } = new List<DtoProductDetail>();
}
