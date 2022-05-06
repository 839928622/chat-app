using Application.Contracts.Persistence;
using MediatR;
using Shared.Common;

namespace Application.Features.Member.Queries.GetMembers
{
    public class GetMembersHandle : IRequestHandler<MemberFilterParams, PaginationResult<MemberToReturnDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetMembersHandle(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <inheritdoc />
        public async Task<PaginationResult<MemberToReturnDto>> Handle(MemberFilterParams request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetMembersAsync(request);
        }
    }
}
