using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Enums.AppUserEntity;

namespace Infrastructure.EntityConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // AppUser
            builder.ToTable(nameof(AppUser));

            builder.HasMany(u => u.UserRoles).WithOne(u => u.AppUser)
                .HasForeignKey(ur => ur.UserId).IsRequired();

            builder.HasIndex(u => u.UserName).IsUnique().HasDatabaseName("IX_UserName");
            builder.Property(u => u.KnownAs).HasMaxLength(256);
            builder.Property(u => u.UserName).HasMaxLength(16);
            builder.Property(u => u.Gender).HasMaxLength(8).HasConversion(c=>c.ToString(),c => Enum.Parse<Gender>(c));
            builder.Property(u => u.Introduction).HasMaxLength(512);
            builder.Property(u => u.LookingFor).HasMaxLength(256);
            builder.Property(u => u.Interests).HasMaxLength(256);
            builder.Property(u => u.City).HasMaxLength(32);
            builder.Property(u => u.Country).HasMaxLength(32);
            
        }
    }
}
