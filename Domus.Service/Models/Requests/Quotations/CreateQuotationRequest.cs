using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Quotations;

public class CreateQuotationRequest
{
	[Required]
    public string CustomerId { get; set; } = null!;

	[Required]
    public string StaffId { get; set; } = null!;

    public DateTime? ExpireAt { get; set; }

    public virtual ICollection<Guid> ProductDetails { get; set; } = new List<Guid>();

    public virtual ICollection<Guid> Services { get; set; } = new List<Guid>();
}
