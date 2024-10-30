using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssReader.Application.Common.Enums;
using RssReader.Domain.Entities.Identity;

namespace RssReader.Infrastructure.Configurations.Identity;

internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .HasMaxLength(100)
               .IsRequired();

        var roles = Enum.GetValues<Roles>()
                        .Select(e => new Role
                        {
                            Id = (sbyte)e,
                            Name = Enum.GetName(e)!
                        })
                        .ToArray();

        builder.HasData(roles);
    }
}
