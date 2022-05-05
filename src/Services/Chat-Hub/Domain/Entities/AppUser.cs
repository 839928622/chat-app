using Microsoft.AspNetCore.Identity;
using Shared.Enums.AppUserEntity;

namespace Domain.Entities
{
    public class AppUser: IdentityUser<int>
    {
      //  public int Id { get; set; }
      //  public string Username { get; set; }
        
      //public  byte[] PasswordHash { get; set; }
      //  public byte[] PasswordSalt { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        /// <summary>
        /// profile that been created
        /// </summary>
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset LastActive { get; set; } = DateTimeOffset.Now;
        public Gender Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
       
        // each user can be multi role , each role can contains multi user
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}