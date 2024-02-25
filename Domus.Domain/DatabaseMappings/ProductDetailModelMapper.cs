using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class ProductDetailModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.ToTable(nameof(ProductDetail));

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
			entity.Property(e => e.DisplayPrice).HasColumnType("float");
			entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.HasOne(d => d.Product).WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.ProductId)    
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(pd => pd.ProductImages).WithOne(pi => pi.ProductDetail)
                .HasForeignKey(pi => pi.ProductDetailId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
