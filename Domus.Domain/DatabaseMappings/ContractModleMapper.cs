using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class ContractModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.ToTable(nameof(Contract));

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastUpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(450);
            entity.Property(e => e.SignedAt).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(256);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ContractCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.ContractLastUpdatedByNavigations)
                .HasForeignKey(d => d.LastUpdatedBy);

            entity.HasOne(d => d.Quotation).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.QuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
