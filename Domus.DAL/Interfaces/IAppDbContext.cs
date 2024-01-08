using Domus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domus.DAL.Interfaces;

public interface IAppDbContext
{ 	
	DbSet<T> CreateSet<T>() where T : BaseEntity;
    void Attach<T>(T entity) where T : BaseEntity;
    void SetModified<T>(T entity) where T : BaseEntity;
    void SetDeleted<T>(T entity) where T : BaseEntity;
    void Refresh<T>(T entity) where T : BaseEntity;
    void Update<T>(T entity) where T : BaseEntity;
    void SaveChanges();
    Task SaveChangesAsync();
}
