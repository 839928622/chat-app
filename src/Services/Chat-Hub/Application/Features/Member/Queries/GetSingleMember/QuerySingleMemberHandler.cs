using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.Member.Queries.GetMembers;
using MediatR;

namespace Application.Features.Member.Queries.GetSingleMember
{
    public class QuerySingleMemberHandler : IRequestHandler<SingleMemberRequest,MemberToReturnDto?>
    {
        private readonly IUserRepository _userRepository;

        public QuerySingleMemberHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <inheritdoc />
        public async Task<MemberToReturnDto?> Handle(SingleMemberRequest request, CancellationToken cancellationToken)
        {

            return await _userRepository.GetMemberInfoById(request.UserId);
        }
    }
}
