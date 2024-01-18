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

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CloseAt).HasColumnType("date");
            entity.Property(e => e.IsClosed).HasDefaultValueSql("((0))");
            entity.Property(e => e.StartAt).HasColumnType("date");

            entity.HasOne(d => d.Quotation).WithOne(p => p.QuotationNegotiationLog)
                .HasForeignKey<Quotation>(d => d.QuotationNegotiationLogId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
