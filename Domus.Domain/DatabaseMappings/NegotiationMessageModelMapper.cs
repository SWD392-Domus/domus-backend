using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class NegotiationMessageModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NegotiationMessage>(entity =>
        {
            entity.ToTable(nameof(NegotiationMessage));

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.SentAt).HasColumnType("date");

			entity.HasOne(d => d.QuotationNegotiationLog)
				.WithMany(d => d.NegotiationMessages)
				.HasForeignKey(d => d.QuotationNegotiationLogId);
        });
    }
}
