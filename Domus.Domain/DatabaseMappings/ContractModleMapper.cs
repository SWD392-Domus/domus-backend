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
            entity.Property(e => e.SignedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(450);
            entity.Property(e => e.Description).HasMaxLength(450);
            entity.Property(e => e.Signature).HasMaxLength(450);
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.Attachments).HasMaxLength(450);
            entity.Property(e => e.Status).HasColumnName("Status");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientContracts)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Contractor).WithMany(p => p.ContractorContracts)
                .HasForeignKey(d => d.ContractorId).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.QuotationRevision).WithOne(p => p.Contract)
                .HasForeignKey<Contract>(d => d.QuotationRevisionId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
