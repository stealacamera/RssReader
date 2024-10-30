using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Configurations;

internal class FeedItemConfiguration : IEntityTypeConfiguration<FeedItem>
{
    public void Configure(EntityTypeBuilder<FeedItem> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Feed)
               .WithMany()
               .HasForeignKey(e => e.FeedId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.ItemId)
               .HasMaxLength(200);

        builder.Property(e => e.Title)
               .HasMaxLength(200);

        builder.Property(e => e.Link)
               .HasMaxLength(300);

        builder.Property(e => e.Author)
               .HasMaxLength(100);
    }
}
