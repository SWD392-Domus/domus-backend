using System.Linq.Expressions;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domus.DAL.Implementations;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly DbSet<Product> _dbSet;
    public ProductRepository(IAppDbContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.CreateSet<Product>();
    }

    public new async Task DeleteManyAsync(Expression<Func<Product, bool>> predicate)
    {
        var entities = _dbSet.Where(predicate);
        await entities.ForEachAsync(p => p.IsDeleted = true);
    }
}
