using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}
