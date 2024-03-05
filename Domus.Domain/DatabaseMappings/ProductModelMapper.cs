using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Domus.Domain.DatabaseMappings;

public class ProductModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable(nameof(Product));

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Brand).HasMaxLength(256);
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.ProductName).HasMaxLength(256);
			entity.Property(e => e.ConcurrencyStamp).IsConcurrencyToken().HasValueGenerator(typeof(StringValueGenerator));

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
