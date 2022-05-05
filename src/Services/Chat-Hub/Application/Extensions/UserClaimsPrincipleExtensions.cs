using System.Security.Claims;

namespace Application.Extensions
{
    public static class UserClaimsPrincipleExtensions
    {
        /// <summary>
        /// return ClaimTypes.Name value, throws an exception if value doesn't exist
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetRequiredUsername(this ClaimsPrincipal user)
        {
            // NameIdentifier = JwtRegisteredClaimNames.NameId
            // Name = JwtRegisteredClaimNames.UniqueName 
            return user.FindFirst(ClaimTypes.Name)?.Value!;
        }

        public static int GetRequiredUserId(this ClaimsPrincipal user)
        {
            // NameIdentifier = JwtRegisteredClaimNames.NameId
            // Name = JwtRegisteredClaimNames.UniqueName 
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }

       
    }
}
