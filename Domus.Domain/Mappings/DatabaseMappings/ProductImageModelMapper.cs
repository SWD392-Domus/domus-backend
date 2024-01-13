using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.Mappings.DatabaseMappings;

public class ProductImageModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.ToTable(nameof(ProductImage));

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ProductDetail).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductIm__Produ__59063A47");
        });
    }
}
