using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class ServiceModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable(nameof(Service));

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.MonetaryUnit).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
        });
    }
}
