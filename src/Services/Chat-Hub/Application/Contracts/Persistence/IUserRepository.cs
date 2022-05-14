using Application.Features.Member.Commands.UpdateUser;
using Application.Features.Member.Queries.GetMembers;
using Domain.Common;
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
        /// get user(member) profile by user id(cache involved) , without sensitive info 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<MemberToReturnDto?> GetMemberInfoById(int userId);
        /// <summary>
        /// remove user cache by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemoveUserCacheById(int userId);

        /// <summary>
        /// update user profile and remove user cache
        ///  
        /// </summary>
        /// <param name="updateUserProfileRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateUserProfile(UpdateUserProfileRequest updateUserProfileRequest, CancellationToken cancellationToken);
    }


}
