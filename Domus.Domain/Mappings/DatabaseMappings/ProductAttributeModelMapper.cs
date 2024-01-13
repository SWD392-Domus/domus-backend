using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.Mappings.DatabaseMappings;

public class ProductAttributeModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductAttribute>(entity =>
        {
            entity.ToTable(nameof(ProductAttribute));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AttributeName).HasMaxLength(256);
        });
    }
}
