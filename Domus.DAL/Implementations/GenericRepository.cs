using Domus.DAL.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Domus.DAL.Implementations;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class 
{
	private readonly IAppDbContext _dbContext;
	private readonly DbSet<T> _dbSet;

    protected GenericRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
		_dbSet = _dbContext.CreateSet<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void AddMany(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public async Task AddManyAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        _dbContext.Update<T>(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Update<T>(entity);
        await Task.CompletedTask;
    }

    public void UpdateMany(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            _dbContext.Update<T>(entity);
        }
    }

    public void UpdateMany(Expression<Func<T, bool>> predicate)
    {
        var entities = _dbSet.Where(predicate);
        foreach (var entity in entities)
        {
            _dbContext.Update<T>(entity);
        }
    }

    public async Task UpdateManyAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            _dbContext.Update<T>(entity);
        }

        await Task.CompletedTask;
    }

    public async Task UpdateManyAsync(Expression<Func<T, bool>> predicate)
    {
        var entities = _dbSet.Where(predicate);
        await entities.ForEachAsync(c => _dbContext.Update<T>(c));
    }

    public void DeleteMany(Expression<Func<T, bool>> predicate)
    {
        var entities = _dbSet.Where(predicate);
        entities.ForEachAsync(c => _dbContext.SetDeleted<T>(c));
    }

    public async Task DeleteManyAsync(Expression<Func<T, bool>> predicate)
    {
        var entities = _dbSet.Where(predicate);
        await entities.ForEachAsync(c => _dbContext.SetDeleted<T>(c));
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<IQueryable<T>> GetAllAsync()
    {
        return await Task.FromResult(_dbSet.AsQueryable());
    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.SingleOrDefault(predicate);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).AsQueryable();
    }

    public async Task<IQueryable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await Task.FromResult(_dbSet.Where(predicate).AsQueryable());
    }

    public long Count(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).Count();
    }

    public async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).CountAsync();
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).Any();
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).AnyAsync();
    }
}
