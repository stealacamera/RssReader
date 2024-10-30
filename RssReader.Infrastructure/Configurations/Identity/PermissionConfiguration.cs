using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssReader.Application.Common.Enums;
using RssReader.Domain.Entities.Identity;

namespace RssReader.Infrastructure.Configurations.Identity;

internal class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .HasMaxLength(100)
               .IsRequired();

        var data = Enum.GetValues<Permissions>()
                        .Select(e => new Permission
                        {
                            Id = (sbyte)e,
                            Name = Enum.GetName(e)!
                        })
                        .ToArray();

        builder.HasData(data);
    }
}
