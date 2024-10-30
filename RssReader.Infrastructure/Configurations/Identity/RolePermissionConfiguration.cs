using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssReader.Application.Common.Enums;
using RssReader.Domain.Entities.Identity;

namespace RssReader.Infrastructure.Configurations.Identity;

internal class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(e => new { e.RoleId, e.PermissionId });

        builder.HasOne<Role>()
               .WithMany()
               .HasForeignKey(e => e.RoleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Permission>()
               .WithMany()
               .HasForeignKey(e => e.PermissionId)
               .OnDelete(DeleteBehavior.Restrict);

        var data = Enum.GetValues<Permissions>()
                       .Select(e => new RolePermission
                       {
                           RoleId = (sbyte)(e == Permissions.CrudFeeds ? Roles.Admin : Roles.User),
                           PermissionId = (sbyte)e
                       })
                       .ToArray();

        builder.HasData(data);
    }
}
