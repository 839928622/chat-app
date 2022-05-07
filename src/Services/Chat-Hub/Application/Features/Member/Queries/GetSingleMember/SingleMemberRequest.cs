using System.ComponentModel.DataAnnotations;
using Application.Features.Member.Queries.GetMembers;
using MediatR;

namespace Application.Features.Member.Queries.GetSingleMember
{
    public class SingleMemberRequest:IRequest<MemberToReturnDto?>
    {
        /// <summary>
        /// any user id
        /// </summary>
        [Required]
        public int UserId { get; set; }
    }
}
