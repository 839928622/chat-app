using System.Net;
using Application.Extensions;
using Application.Features.Message.Queries.MessageForUser;
using Application.Features.Message.Queries.RecentMessagesBetweenTwoUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace ChatHub.API.Controllers
{
    [Authorize]
    public class MessageController : BaseApiController
    {
        private readonly IMediator _mediator;
        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///  get recent message(s) between two users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("RecentMessages")]
        [ProducesResponseType(typeof(PaginationResult<MessageDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> RecentMessages([FromQuery]RecentMessagesRequest request)
        {
            request.CurrentUserId = User.GetRequiredUserId();
            var res = await _mediator.Send(request);
            return Ok(res);
        }
    }
}
