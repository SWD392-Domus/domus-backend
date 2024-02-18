using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Domus.Domain.DatabaseMappings;

public class PackageImageModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PackageImage>(entity =>
        {
            entity.ToTable(nameof(PackageImage));

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Package).WithMany(p => p.PackageImages)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}