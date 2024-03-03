using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class ProductDetailQuotationRevisionModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductDetailQuotationRevision>(entity =>
        {
            entity.ToTable("ProductDetail_QuotationRevision");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            // entity.Property(e => e.Version).HasDefaultValueSql("((0))");

			entity.HasOne(d => d.QuotationRevision).WithMany(p => p.ProductDetailQuotationRevisions)
				.HasForeignKey(d => d.QuotationRevisionId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			entity.HasOne(d => d.ProductDetail).WithMany(p => p.ProductDetailQuotationRevisions)
				.HasForeignKey(d => d.ProductDetailId)
				.OnDelete(DeleteBehavior.ClientSetNull);

            // entity.HasOne(d => d.ProductDetailQuotation).WithMany(p => p.ProductDetailQuotationRevisions)
            //     .HasForeignKey(d => d.ProductDetailQuotationId)
            //     .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
