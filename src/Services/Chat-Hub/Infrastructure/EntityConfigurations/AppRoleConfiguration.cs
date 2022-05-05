using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    internal class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            // AppRole\
            builder.ToTable(nameof(AppRole));

            builder.HasMany(ur => ur.UserRoles).WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId).IsRequired();


        }
    }
}
