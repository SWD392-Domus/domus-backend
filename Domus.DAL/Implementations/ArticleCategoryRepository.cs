using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ArticleCategoryRepository : GenericRepository<ArticleCategory>, IArticleCategoryRepository
{
    public ArticleCategoryRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}
