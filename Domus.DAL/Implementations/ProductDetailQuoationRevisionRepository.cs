using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ProductDetailQuoationRevisionRepository : GenericRepository<ProductDetailQuotationRevision>, IProductDetailQuotationRevisionRepository
{
    public ProductDetailQuoationRevisionRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}