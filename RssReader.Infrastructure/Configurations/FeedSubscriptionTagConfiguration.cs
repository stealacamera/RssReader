using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Configurations;

internal class FeedSubscriptionTagConfiguration : IEntityTypeConfiguration<FeedSubscriptionTag>
{
    public void Configure(EntityTypeBuilder<FeedSubscriptionTag> builder)
    {
        builder.HasKey(e => new { e.TagId, e.FeedSubscriptionId });

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.HasOne<Tag>()
               .WithMany()
               .HasForeignKey(e => e.TagId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<FeedSubscription>()
               .WithMany()
               .HasForeignKey(e => e.FeedSubscriptionId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
