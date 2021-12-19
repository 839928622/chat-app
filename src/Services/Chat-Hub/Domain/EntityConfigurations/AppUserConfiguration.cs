using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // AppUser
            builder.HasMany(ur => ur.UserRoles).WithOne(u => u.AppUser)
                .HasForeignKey(ur => ur.UserId).IsRequired();
        }
    }
}
