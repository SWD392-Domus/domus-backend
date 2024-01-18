using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ArticleRepository : GenericRepository<Article>, IArticleRepository
{
    public ArticleRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}
