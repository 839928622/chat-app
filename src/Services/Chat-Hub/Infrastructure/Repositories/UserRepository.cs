using Application.Contracts.Persistence;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository :RepositoryBase<AppUser>, IUserRepository
    {
        public UserRepository(ChatAppContext dbxContext) : base(dbxContext)
        {

        }


        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await ChatAppDbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<AppUser> GetRequiredUserByIdAsync(int id)
        {
            return await ChatAppDbContext.Users.SingleAsync(x => x.Id == id);
        }
    }
}
