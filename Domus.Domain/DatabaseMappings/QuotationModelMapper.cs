using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Domus.Domain.DatabaseMappings;

public class QuotationModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quotation>(entity =>
        {
            entity.ToTable(nameof(Quotation));

			entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnType("date");
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.CustomerId).HasMaxLength(450);
            entity.Property(e => e.ExpireAt).HasColumnType("date");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastUpdatedAt).HasColumnType("date");
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(450);
            entity.Property(e => e.StaffId).HasMaxLength(450);
            entity.Property(e => e.Status).HasMaxLength(256);
			entity.Property(e => e.ConcurrencyStamp).IsConcurrencyToken().HasValueGenerator(typeof(StringValueGenerator));

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.QuotationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Customer).WithMany(p => p.QuotationCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.QuotationLastUpdatedByNavigations)
                .HasForeignKey(d => d.LastUpdatedBy);

            entity.HasOne(d => d.QuotationNegotiationLog)
				.WithOne(d => d.Quotation)
                .HasForeignKey<QuotationNegotiationLog>(d => d.QuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Staff).WithMany(p => p.QuotationStaffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(d => d.Services).WithMany(p => p.Quotations)
                .UsingEntity<Dictionary<string, object>>(
                    "QuotationService",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Quotation>().WithMany()
                        .HasForeignKey("QuotationId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("QuotationId", "ServiceId");
                        j.ToTable("QuotationService");
                    });
        });
    }
}
