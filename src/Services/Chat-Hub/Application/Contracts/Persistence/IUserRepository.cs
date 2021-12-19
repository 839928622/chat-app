using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface IUserRepository : IAsyncRepository<AppUser>
    {

    }
}
