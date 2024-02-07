using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class PackageModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
		modelBuilder.Entity<Package>(entity => 
		{
			entity.ToTable(nameof(Package));
			entity.HasMany(e => e.Services).WithMany(e => e.Packages)  
				.UsingEntity<Dictionary<string, object>>(
					"Package_Service",
					r => r.HasOne<Service>().WithMany()
						.HasForeignKey("ServiceId")
						.OnDelete(DeleteBehavior.ClientSetNull),
					l => l.HasOne<Package>().WithMany()
						.HasForeignKey("PackageId")
						.OnDelete(DeleteBehavior.ClientSetNull),
					j =>
					{
						j.HasKey("PackageId", "ServiceId");
						j.ToTable("Package_Service");
					});
			entity.HasMany(e => e.ProductDetails).WithMany(e => e.Packages)  
				.UsingEntity<Dictionary<string, object>>(
					"Package_ProductDetail",
					r => r.HasOne<ProductDetail>().WithMany()
						.HasForeignKey("ProductDetailId")
						.OnDelete(DeleteBehavior.ClientSetNull),
					l => l.HasOne<Package>().WithMany()
						.HasForeignKey("PackageId")
						.OnDelete(DeleteBehavior.ClientSetNull),
					j =>
					{
						j.HasKey("PackageId", "ProductDetailId");
						j.ToTable("Package_ProductDetail");
					});
			entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
			entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(256);
			entity.Property(e => e.Discount).HasColumnType("float");
		});
    }
}
