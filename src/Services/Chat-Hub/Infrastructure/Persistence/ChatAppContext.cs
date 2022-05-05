using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence
{
    #pragma warning disable CS8618
    public class ChatAppContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
       IdentityUserToken<int>>
    {
        public ChatAppContext(DbContextOptions options) : base(options)
        {

        }
        //public DbSet<AppUser> Users { get; set; }
        //public DbSet<UserLike> Likes { get; set; }

        public DbSet<Message> Message { get; set; }

        public DbSet<Photo> Photo { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // sqlite doesn't support DateTimeOffset

            base.OnModelCreating(modelBuilder);
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType
                                                                                   == typeof(decimal));
                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>(); // cuz sqlite doesn't support double column type,if you orderby decimal,it will throw an error
                    }
                    // sqlite doesn't support DateTimeOffset
                    var dateTimeProperties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset));
                    foreach (var property in dateTimeProperties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatAppContext).Assembly);


         
           

          
           

        }
    }
}
