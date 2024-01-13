using Domus.Common.Helpers;
using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domus.DAL.Data;

public partial class DomusContext : IdentityDbContext<DomusUser>
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

		optionsBuilder.UseSqlServer(DataAccessHelper.GetDefaultConnectionString());
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
}
