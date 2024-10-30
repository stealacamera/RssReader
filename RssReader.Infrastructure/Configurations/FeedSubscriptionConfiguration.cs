using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Configurations;

internal class FeedSubscriptionConfiguration : IEntityTypeConfiguration<FeedSubscription>
{
    public void Configure(EntityTypeBuilder<FeedSubscription> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .HasMaxLength(80)
               .IsRequired();

        builder.HasIndex(e => new { e.FeedId, e.FolderId })
               .IsUnique();

        builder.HasOne<Feed>()
               .WithMany()
               .HasForeignKey(e => e.FeedId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Folder)
               .WithMany()
               .HasForeignKey(e => e.FolderId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

    }
}
