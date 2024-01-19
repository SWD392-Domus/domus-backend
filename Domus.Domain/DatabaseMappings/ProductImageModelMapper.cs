using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class ProductImageModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.ToTable(nameof(ProductImage));

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.ProductDetail).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
