using Domus.Domain.Entities.Base;

namespace Domus.Domain.Entities;

public partial class Package : DeletableEntity<Guid>
{
	public string Name { get; set; } = null!;

	public double Discount { get; set; }

	public virtual ICollection<Service> Services { get; set; } = new List<Service>();
	public virtual ICollection<PackageProductDetail> PackageProductDetails { get; set; } = new List<PackageProductDetail>();
	public virtual ICollection<PackageImage> PackageImages { get; set; } = new List<PackageImage>();
	public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}
