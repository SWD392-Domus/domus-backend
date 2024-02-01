using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Domus.Domain.DatabaseMappings;

public class DomusUserModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DomusUser>(entity =>
        {
            entity.ToTable(nameof(DomusUser));
            entity.Property(e => e.ProfileImage).HasMaxLength(256);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.ConcurrencyStamp).IsConcurrencyToken().HasValueGenerator<StringValueGenerator>();
        });
    }
}
