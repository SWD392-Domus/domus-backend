using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class QuotationNegotiationLogModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuotationNegotiationLog>(entity =>
        {
            entity.ToTable(nameof(QuotationNegotiationLog));

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CloseAt).HasColumnType("datetime");
            entity.Property(e => e.IsClosed).HasDefaultValueSql("((0))");
            entity.Property(e => e.StartAt).HasColumnType("datetime");

            entity.HasOne(d => d.Quotation).WithOne(p => p.QuotationNegotiationLog).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
