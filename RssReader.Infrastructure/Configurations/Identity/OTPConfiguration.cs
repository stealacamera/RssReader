using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssReader.Domain.Entities.Identity;

namespace RssReader.Infrastructure.Configurations.Identity;

internal class OTPConfiguration : IEntityTypeConfiguration<OTP>
{
    public void Configure(EntityTypeBuilder<OTP> builder)
    {
        builder.HasKey(e => e.UserId);

        builder.Property(e => e.Password)
               .IsRequired()
               .HasMaxLength(6);

        builder.Property(e => e.RetryAttempts)
               .IsRequired()
               .HasDefaultValue(0);

        builder.Property(e => e.ExpiryDate)
               .IsRequired();

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<OTP>(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
