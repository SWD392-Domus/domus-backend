using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.Mappings.DatabaseMappings;

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
            entity.Property(e => e.CreatedAt).HasColumnType("date");
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastUpdatedAt).HasColumnType("date");
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(450);
            entity.Property(e => e.ProductName).HasMaxLength(256);
            entity.Property(e => e.Style).HasMaxLength(256);
            entity.Property(e => e.WeightUnit).HasMaxLength(256);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Categor__5441852A");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Created__5535A963");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.ProductLastUpdatedByNavigations)
                .HasForeignKey(d => d.LastUpdatedBy)
                .HasConstraintName("FK__Product__LastUpd__5629CD9C");
        });
    }
}
