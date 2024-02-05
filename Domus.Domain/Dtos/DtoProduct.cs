namespace Domus.Domain.Dtos;

public class DtoProduct
{
	public Guid Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Brand { get; set; }

    public string? Description { get; set; }

    public DtoProductCategory ProductCategory { get; set; } = null!;

    public ICollection<DtoProductDetail> ProductDetails { get; set; } = new List<DtoProductDetail>();
}
