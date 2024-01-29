using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Package : DeletableEntity<Guid>
{
	public string Name { get; set; } = null!;

	public double Discount { get; set; }

	public virtual ICollection<Service> Services { get; set; } = new List<Service>();

	public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
