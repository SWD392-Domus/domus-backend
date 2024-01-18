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

            entity.HasOne(d => d.Quotation).WithMany(p => p.QuotationNegotiationLogs)
                .HasForeignKey(d => d.QuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Quotation__Quota__6EF57B66");
        });
    }
}