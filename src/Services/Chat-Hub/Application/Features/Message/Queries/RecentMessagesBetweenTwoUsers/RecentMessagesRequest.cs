using System.ComponentModel.DataAnnotations;
using Application.Features.Message.Queries.MessageForUser;
using MediatR;
using Shared.Common;

namespace Application.Features.Message.Queries.RecentMessagesBetweenTwoUsers
{
    public class RecentMessagesRequest : PaginationRequestParams,IRequest<PaginationResult<MessageDto>>
    {
        /// <summary>
        /// The user that currently has logged in
        /// </summary>
        public int CurrentUserId { get; set; }
        /// <summary>
        /// The user that I am chatting with
        /// </summary>
        [Required]
        public int AnotherUserId { get; set; }
    }
}
