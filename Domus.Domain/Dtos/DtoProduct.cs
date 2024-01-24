namespace Domus.Domain.Dtos;

public class DtoProduct
{
	public Guid Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Color { get; set; }

    public double? Weight { get; set; }

    public string? WeightUnit { get; set; }

    public string? Style { get; set; }

    public string? Brand { get; set; }

    public string? Description { get; set; }

    public DtoProductCategory ProductCategory { get; set; } = null!;

    public ICollection<DtoProductDetail> ProductDetails { get; set; } = new List<DtoProductDetail>();
}
