﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Configurations;

internal class FolderConfiguration : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(40);

        builder.HasIndex(e => new { e.Name, e.OwnerId })
               .IsUnique();

        builder.HasOne<Folder>()
               .WithMany()
               .HasForeignKey(e => e.ParentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
               .WithMany()
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
