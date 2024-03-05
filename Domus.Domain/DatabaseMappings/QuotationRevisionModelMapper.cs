using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class QuotationRevisionModelMapper : IDatabaseModelMapper
{
	public void Map(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<QuotationRevision>(entity =>
		{
			entity.ToTable("QuotationRevision");

			entity.Property(e => e.Id).ValueGeneratedOnAdd();
			entity.Property(e => e.CreatedAt).HasColumnType("datetime");

			entity.HasOne(d => d.Quotation).WithMany(p => p.QuotationRevisions)
				.HasForeignKey(d => d.QuotationId)
				.OnDelete(DeleteBehavior.ClientSetNull);
		});
	}
}
