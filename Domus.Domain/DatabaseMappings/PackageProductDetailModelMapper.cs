using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class PackageProductDetailModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PackageProductDetail>(entity =>
        {
            entity.ToTable("Package_ProductDetail");

            entity.HasNoKey();
            entity.HasKey(e => new { e.PackageId, e.ProductDetailId });
            entity.Property(e => e.Quantity).HasDefaultValue(0);

            entity.HasOne(e => e.Package).WithMany(pk => pk.PackageProductDetails)
                .HasForeignKey(e => e.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            entity.HasOne(e => e.ProductDetail).WithMany(pk => pk.PackageProductDetail)
                .HasForeignKey(e => e.ProductDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}