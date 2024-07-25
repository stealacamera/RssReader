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

        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(80);

        builder.HasOne<Folder>()
            .WithMany()
            .HasForeignKey(e => e.FolderId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
