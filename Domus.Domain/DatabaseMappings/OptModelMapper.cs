using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class OptModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Otp>(entity =>
        {
            entity.ToTable(nameof(Otp));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Code).HasMaxLength(256);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Used).HasDefaultValueSql("((0))");

            entity.HasOne(e => e.User)
                .WithMany(u => u.OtpCodes)
                .HasForeignKey(e => e.UserId);
        });
    }
}