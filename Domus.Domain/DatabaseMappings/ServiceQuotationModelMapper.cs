using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class ServiceQuotationModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
		modelBuilder.Entity<ServiceQuotation>(entity =>
		{
			entity.ToTable("Service_Quotation");
			entity.HasKey(e => new { e.QuotationId, e.ServiceId });

			entity.HasOne(d => d.Quotation)
				.WithMany(p => p.ServiceQuotations)
				.HasForeignKey(d => d.QuotationId)
				.OnDelete(DeleteBehavior.ClientSetNull);
			
			entity.HasOne(d => d.Service)
				.WithMany(p => p.ServiceQuotations)
				.HasForeignKey(d => d.ServiceId)
				.OnDelete(DeleteBehavior.ClientSetNull);
		});
    }
}
