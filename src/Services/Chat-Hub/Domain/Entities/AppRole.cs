using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole()
        {
        }
        public AppRole(string roleName):base(roleName)
        {
            
        }

        // each user can be multi role , each role can contains multi user
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
