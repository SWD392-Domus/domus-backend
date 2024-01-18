using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Package : BaseEntity<Guid>
{
	public string Name { get; set; } = null!;

	public virtual ICollection<Service> Services { get; set; } = new List<Service>();

	public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
