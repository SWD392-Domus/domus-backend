using System.Linq.Expressions;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domus.DAL.Implementations;

public class QuotationRepository : GenericRepository<Quotation>, IQuotationRepository
{
	private readonly DbSet<Quotation> _dbSet;

	public QuotationRepository(IAppDbContext context) : base(context)
	{
		_dbSet = context.CreateSet<Quotation>();
	}

    public new async Task DeleteManyAsync(Expression<Func<Quotation, bool>> predicate)
    {
        var entities = _dbSet.Where(predicate);
        await entities.ForEachAsync(p => p.IsDeleted = true);
    }
}
