using Domus.Domain.Entities;
using Domus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.DatabaseMappings;

public class NotificationModelMapper:IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable(nameof(Notification));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Content).HasColumnName("Content");
            entity.Property(e => e.SentAt);
            entity.Property(e => e.Status).HasColumnName("Status").HasColumnType("INT");
            entity.Property(e => e.RedirectString).HasColumnName("RedirectString");
            entity.Property(e => e.Image).HasColumnName("Image");

            entity.HasOne(e => e.Recipient)
                .WithMany(r => r.Notifications)
                .HasForeignKey(e => e.RecipientId).IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);

        });

    }
}