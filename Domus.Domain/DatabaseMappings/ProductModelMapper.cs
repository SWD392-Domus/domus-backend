using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class ProductModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable(nameof(Product));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Brand).HasMaxLength(256);
            entity.Property(e => e.Color).HasMaxLength(256);
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.ProductName).HasMaxLength(256);
            entity.Property(e => e.Style).HasMaxLength(256);
            entity.Property(e => e.WeightUnit).HasMaxLength(256);

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Product__5FB337D6");
        });
    }
}
