using Domain.Entities;

namespace Application.Contracts.Infrastructure
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
