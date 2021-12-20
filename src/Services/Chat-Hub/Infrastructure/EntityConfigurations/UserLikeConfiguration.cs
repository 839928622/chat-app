using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder.HasKey(k => new { k.SourceUserId, k.LikeUserId });
            builder.HasOne(s => s.SourceUser)
                                           .WithMany(l => l.LikedUsers)
                                           .HasForeignKey(s => s.SourceUserId)
                                           .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.LikedUser)
                                           .WithMany(l => l.LikedByUsers)
                                           .HasForeignKey(s => s.LikeUserId)
                                           .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
