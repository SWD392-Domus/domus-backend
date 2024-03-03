using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class QuotationRevisionRepository : GenericRepository<QuotationRevision>, IQuotationRevisionRepository
{
    public QuotationRevisionRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}