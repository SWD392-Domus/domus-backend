using Microsoft.AspNetCore.Identity;

namespace Domus.Domain.Entities;

public partial class DomusUser : IdentityUser
{
    public string? ProfileImage { get; set; }

    public bool IsDeleted { get; set; }
    
    public virtual ICollection<Article> ArticleCreatedByNavigations { get; set; } = new List<Article>();

    public virtual ICollection<Article> ArticleLastUpdatedByNavigations { get; set; } = new List<Article>();

    public virtual ICollection<Contract> ContractCreatedByNavigations { get; set; } = new List<Contract>();

    public virtual ICollection<Contract> ContractLastUpdatedByNavigations { get; set; } = new List<Contract>();

    public virtual ICollection<Quotation> QuotationCreatedByNavigations { get; set; } = new List<Quotation>();

    public virtual ICollection<Quotation> QuotationCustomers { get; set; } = new List<Quotation>();

    public virtual ICollection<Quotation> QuotationLastUpdatedByNavigations { get; set; } = new List<Quotation>();

    public virtual ICollection<Quotation> QuotationStaffs { get; set; } = new List<Quotation>();
    
}
