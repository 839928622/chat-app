using System.Net;
using Application.Extensions;
using Application.Features.Account.Commands.CreateNewUser;
using Application.Features.Member.Commands.UpdateUser;
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

        #region user/member photos
        /// <summary>
        /// get all photos by user id 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("UserPhotos/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<PhotoDto>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> UserPhotos([FromRoute] QueryUserPhotoRequest input)
        {
            var res = await _mediator.Send(input);
            return Ok(res);
        }




        #endregion

        #region Member
        /// <summary>
        /// filter members
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetMembers")]
        [ProducesResponseType(typeof(PaginationResult<MemberToReturnDto>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<PaginationResult<MemberToReturnDto>>> GetMembers([FromQuery] MemberFilterParams request)
        {

            request.CurrentUserId = User.GetRequiredUserId();
            var res = await _mediator.Send(request);
            return Ok(res);

        }

        /// <summary>
        /// get specific user  profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(MemberToReturnDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationResult<MemberToReturnDto>>> Get([FromRoute]SingleMemberRequest request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }


        [HttpPut("UpdateMemberProfile")]
        public async Task<ActionResult> UpdateMemberProfile(UpdateUserProfileRequest request)
        {
            request.CurrentUserId = User.GetRequiredUserId();
            await _mediator.Send(request);
            return NoContent();
        }
        #endregion


    }
}
