using Domus.Domain.Entities;
using Domus.DAL.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Domus.DAL.Implementations;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity 
{
	private readonly IAppDbContext _dbContext;

    protected GenericRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(T entity)
    {
        _dbContext.CreateSet<T>().AddAsync(entity);
    }

    public async Task AddAsync(T entity)
    {
        await _dbContext.CreateSet<T>().AddAsync(entity);
    }

    public void AddMany(IEnumerable<T> entities)
    {
        _dbContext.CreateSet<T>().AddRange(entities);
    }

    public async Task AddManyAsync(IEnumerable<T> entities)
    {
        await _dbContext.CreateSet<T>().AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        _dbContext.Update<T>(entity);
    }

    public Task UpdateAsync(T entity)
    {
        _dbContext.Update<T>(entity);
        return Task.CompletedTask;
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
        var entities = _dbContext.CreateSet<T>().Where(predicate);
        foreach (var entity in entities)
        {
            _dbContext.Update<T>(entity);
        }
    }

    public Task UpdateManyAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            _dbContext.Update<T>(entity);
        }

        return Task.CompletedTask;
    }

    public async Task UpdateManyAsync(Expression<Func<T, bool>> predicate)
    {
        var entities = _dbContext.CreateSet<T>().Where(predicate);
        await entities.ForEachAsync(c => _dbContext.Update<T>(c));
    }

    public void DeleteMany(Expression<Func<T, bool>> predicate)
    {
        var entities = _dbContext.CreateSet<T>().Where(predicate);
        entities.ForEachAsync(c => _dbContext.SetDeleted<T>(c));
    }

    public async Task DeleteManyAsync(Expression<Func<T, bool>> predicate)
    {
        var entities = _dbContext.CreateSet<T>().Where(predicate);
        await entities.ForEachAsync(c => _dbContext.SetDeleted<T>(c));
    }

    public IQueryable<T> GetAll()
    {
        return _dbContext.CreateSet<T>().AsQueryable();
    }

    public async Task<IQueryable<T>> GetAllAsync()
    {
        return await Task.FromResult(_dbContext.CreateSet<T>().AsQueryable());
    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.CreateSet<T>().SingleOrDefault(predicate);
    }

    public Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.CreateSet<T>().FirstOrDefaultAsync(predicate);
    }

    public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.CreateSet<T>().Where(predicate).AsQueryable();
    }

    public async Task<IQueryable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await Task.FromResult(_dbContext.CreateSet<T>().Where(predicate).AsQueryable());
    }

    public long Count(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.CreateSet<T>().Where(predicate).Count();
    }

    public async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.CreateSet<T>().Where(predicate).CountAsync();
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.CreateSet<T>().Where(predicate).Any();
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.CreateSet<T>().Where(predicate).AnyAsync();
    }
}
