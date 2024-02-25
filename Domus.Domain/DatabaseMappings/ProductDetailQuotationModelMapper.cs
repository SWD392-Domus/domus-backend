using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class ProductDetailQuotationModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductDetailQuotation>(entity =>
        {
            entity.ToTable("ProductDetail_Quotation");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.MonetaryUnit).HasMaxLength(256);
            entity.Property(e => e.QuantityType).HasMaxLength(256);

            entity.HasOne(d => d.ProductDetail).WithMany(p => p.ProductDetailQuotations)
                .HasForeignKey(d => d.ProductDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Quotation).WithMany(p => p.ProductDetailQuotations)
                .HasForeignKey(d => d.QuotationId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
