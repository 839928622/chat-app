using System.Net;
using Application.Extensions;
using Application.Features.Account.Commands.CreateNewUser;
using Application.Features.Member.Queries.GetMembers;
using Application.Features.Member.Queries.GetSingleMember;
using Application.Features.Photo.Queries.QueryUserPhotos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace ChatHub.API.Controllers
{
    [Authorize]
    public class MemberController : BaseApiController
    {
        private readonly IMediator _mediator;

        public MemberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region user photos

        [HttpGet("UserPhotos/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<PhotoDto>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> UserPhotos([FromRoute] QueryUserPhotoRequest input)
        {
            var res = await _mediator.Send(input);
            return Ok(res);
        }



        #endregion

        #region Member
        [HttpGet("GetMembers")]
        [ProducesResponseType(typeof(PaginationResult<MemberToReturnDto>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<PaginationResult<MemberToReturnDto>>> GetMembers([FromQuery] MemberFilterParams request)
        {

            request.CurrentUserId = User.GetRequiredUserId();
            var res = await _mediator.Send(request);
            return Ok(res);

        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(MemberToReturnDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationResult<MemberToReturnDto>>> Get([FromRoute]SingleMemberRequest request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }

        #endregion


    }
}
