using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Configurations;

internal class FeedTagConfiguration : IEntityTypeConfiguration<FeedTag>
{
    public void Configure(EntityTypeBuilder<FeedTag> builder)
    {
        builder.HasKey(e => new { e.TagId, e.FeedId });

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.HasOne<Tag>()
               .WithMany()
               .HasForeignKey(e => e.TagId)
               .IsRequired();

        builder.HasOne<Feed>()
               .WithMany()
               .HasForeignKey(e => e.FeedId)
               .IsRequired();
    }
}
