using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domus.DAL.Data;

public partial class DomusContext : IdentityDbContext<DomusUser>, IAppDbContext
{
    public DomusContext()
    {
    }

    public DomusContext(DbContextOptions<DomusContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);

		if (optionsBuilder.IsConfigured)
			return;

		optionsBuilder.UseSqlServer(DataAccessHelper.GetDefaultConnectionString())
			.EnableSensitiveDataLogging()
			.LogTo(Console.WriteLine);
	}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
		base.OnModelCreating(modelBuilder);

		var modelMappers = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(a => a.GetTypes())
			.Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(IDatabaseModelMapper)));

		foreach (var modelMapper in modelMappers)
		{
			var instance = Activator.CreateInstance(modelMapper) as IDatabaseModelMapper;
			instance?.Map(modelBuilder);
		}

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    public DbSet<T> CreateSet<T>() where T : class
    {
	    return base.Set<T>();
    }

    public new void Attach<T>(T entity) where T : class
    {
	    base.Attach(entity);
    }

    public void SetModified<T>(T entity) where T : class
    {
	    base.Entry(entity).State = EntityState.Modified;
    }

    public void SetDeleted<T>(T entity) where T : class
    {
	    base.Entry(entity).State = EntityState.Deleted;
    }

    public void Refresh<T>(T entity) where T : class
    {
	    base.Entry(entity).Reload();
    }

    public new void Update<T>(T entity) where T : class
    {
	    base.Update(entity);
    }

    public new void SaveChanges()
    {
	    base.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
	    await base.SaveChangesAsync();
    }
}
