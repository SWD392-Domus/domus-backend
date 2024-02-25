using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class ArticleModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable(nameof(Article));

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnType("date");
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastUpdatedAt).HasColumnType("date");
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(450);
            entity.Property(e => e.Title).HasMaxLength(256);

            entity.HasOne(d => d.ArticleCategory).WithMany(p => p.Articles)
                .HasForeignKey(d => d.ArticleCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ArticleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.ArticleLastUpdatedByNavigations)
                .HasForeignKey(d => d.LastUpdatedBy);
        });
    }
}
