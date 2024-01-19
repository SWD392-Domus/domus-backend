using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class QuotationStatusModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuotationStatus>(entity =>
        {
            entity.ToTable(nameof(QuotationStatus));

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.StatusType).HasMaxLength(256);
        });
    }
}
