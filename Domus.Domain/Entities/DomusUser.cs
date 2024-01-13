using Microsoft.AspNetCore.Identity;

namespace Domus.Domain.Entities;

public partial class DomusUser : IdentityUser
{
    public virtual ICollection<Article> ArticleCreatedByNavigations { get; set; } = new List<Article>();

    public virtual ICollection<Article> ArticleLastUpdatedByNavigations { get; set; } = new List<Article>();

    public virtual ICollection<Category> CategoryCreatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Category> CategoryLastUpdatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductLastUpdatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Quotation> QuotationCreatedByNavigations { get; set; } = new List<Quotation>();

    public virtual ICollection<Quotation> QuotationCustomers { get; set; } = new List<Quotation>();

    public virtual ICollection<Quotation> QuotationLastUpdatedByNavigations { get; set; } = new List<Quotation>();

    public virtual ICollection<Quotation> QuotationStaffs { get; set; } = new List<Quotation>();
}
