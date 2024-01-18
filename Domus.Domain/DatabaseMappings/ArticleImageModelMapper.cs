using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class ArticleImageModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArticleImage>(entity =>
        {
            entity.ToTable(nameof(ArticleImage));

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleIm__Artic__6E01572D");
        });
    }
}