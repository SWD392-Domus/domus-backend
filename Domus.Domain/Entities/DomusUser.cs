using System.Collections;
using Microsoft.AspNetCore.Identity;

namespace Domus.Domain.Entities;

public partial class DomusUser : IdentityUser
{
    public string? ProfileImage { get; set; }

    public bool IsDeleted { get; set; }

	public string FullName { get; set; } = null!;

	public string? Address { get; set; }

	public string Gender { get; set; } = "N/A";
	
    public virtual ICollection<Article> ArticleCreatedByNavigations { get; set; } = new List<Article>();

    public virtual ICollection<Article> ArticleLastUpdatedByNavigations { get; set; } = new List<Article>();
    
    public virtual ICollection<Quotation> QuotationCreatedByNavigations { get; set; } = new List<Quotation>();

    public virtual ICollection<Quotation> QuotationCustomers { get; set; } = new List<Quotation>();

    public virtual ICollection<Quotation> QuotationLastUpdatedByNavigations { get; set; } = new List<Quotation>();

    public virtual ICollection<Quotation> QuotationStaffs { get; set; } = new List<Quotation>();
    public virtual ICollection<Contract> ClientContracts { get; set; } = new List<Contract>();
    public virtual ICollection<Contract> ContractorContracts { get; set; } = new List<Contract>();
    public ICollection<Otp> OtpCodes { get; set; } = new List<Otp>();

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

}
