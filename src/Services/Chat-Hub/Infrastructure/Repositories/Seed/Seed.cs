using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Enums.AppRole;

namespace Infrastructure.Repositories.Seed
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ChatAppContext appContext)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("../Infrastructure/Repositories/Seed/UserSeedData.json");
            
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData,  new JsonSerializerOptions()
            {
                Converters = { new JsonStringEnumConverter() }
            });

            foreach (var user in users!)
            {
                //using var hmac = new HMACSHA512();
                //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Password"));

                //user.PasswordSalt = hmac.Key;

                await userManager.CreateAsync(user, "Password");
            }

            // seed photo
            var photosJson = await System.IO.File.ReadAllTextAsync("../Infrastructure/Repositories/Seed/PhotoSeed.json");

            var photos = JsonSerializer.Deserialize<List<Photo>>(photosJson);
            await appContext.Photo.AddRangeAsync(photos!); 
            await appContext.SaveChangesAsync();



            var roles = new List<AppRole>()
            {
                new AppRole(nameof(Roles.Member)),
                new AppRole(nameof(Roles.Moderator)),
                new AppRole(nameof(Roles.Admin)),
            };
            foreach (var role in roles)
            {
               await roleManager.CreateAsync(role);
            }
            

            foreach (var user in users)
            {
              await  userManager.AddToRoleAsync(user, nameof(Roles.Member));
            }
            

            // admin
            var admin = new AppUser()
            {
                UserName = "Admin"
            };
            await userManager.CreateAsync(admin, "Password");
            await userManager.AddToRolesAsync(admin, new List<string>() { nameof(Roles.Admin), nameof(Roles.Moderator) });
            //await context.SaveChangesAsync();

        }
    }
}
