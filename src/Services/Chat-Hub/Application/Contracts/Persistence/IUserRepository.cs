using Application.Features.Member.Queries.GetMembers;
using Domain.Entities;
using Shared.Common;

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

        /// <summary>
        /// GetMembers(users)
        /// </summary>
        /// <param name="memberFilter"></param>
        /// <returns></returns>
        Task<PaginationResult<MemberToReturnDto>> GetMembersAsync(MemberFilterParams memberFilter);
        /// <summary>
        /// get user(member) profile by user id , without sensitive info 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<MemberToReturnDto?> GetMemberInfoById(int userId);
    }


}
