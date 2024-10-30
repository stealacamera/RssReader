using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Configurations;

internal class FeedConfiguration : IEntityTypeConfiguration<Feed>
{
    public void Configure(EntityTypeBuilder<Feed> builder)
    {
        builder.HasKey(e => e.Id);
        

        builder.Property(e => e.Url)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasIndex(e => e.Url)
               .IsUnique();


        builder.Property(e => e.Name)
               .HasMaxLength(80)
               .IsRequired();

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.Property(e => e.Update_ETag)
               .HasMaxLength(2000);

        builder.Property(e => e.Update_LastModified)
               .HasMaxLength(35);

        builder.Property(e => e.IconUrl)
               .HasMaxLength(600);
    }
}
