using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssReader.Domain.Entities.Identity;

namespace RssReader.Infrastructure.Configurations.Identity;

internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(e => new { e.UserId, e.RoleId });

        builder.HasOne<User>()
               .WithOne()
               .HasForeignKey<UserRole>(e => e.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Role>()
               .WithMany()
               .HasForeignKey(e => e.RoleId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
