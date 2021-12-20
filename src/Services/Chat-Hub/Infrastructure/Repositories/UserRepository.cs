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

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await DbContext.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);
        }
    }
}
