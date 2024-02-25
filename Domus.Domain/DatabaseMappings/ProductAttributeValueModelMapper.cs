using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class ProductAttributeValueModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductAttributeValue>(entity =>
        {
            entity.ToTable(nameof(ProductAttributeValue));

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Value).HasMaxLength(256);
            entity.Property(e => e.ValueType).HasMaxLength(256);

            entity.HasOne(d => d.ProductAttribute).WithMany(p => p.ProductAttributeValues)
                .HasForeignKey(d => d.ProductAttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ProductDetail).WithMany(p => p.ProductAttributeValues)
                .HasForeignKey(d => d.ProductDetailId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
