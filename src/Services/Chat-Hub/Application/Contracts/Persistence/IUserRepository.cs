using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface IUserRepository : IAsyncRepository<AppUser>
    {
        Task<AppUser?> GetUserByUsernameAsync(string username);
        /// <summary>
        /// get required user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppUser> GetRequiredUserByIdAsync(int id);
    }


}
