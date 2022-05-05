using System.Net;
using Application.Features.Account.Commands.CreateNewUser;
using Application.Features.Photo.Queries.QueryUserPhotos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatHub.API.Controllers
{
   // [Authorize]
    public class MemberController : BaseApiController
    {
        private readonly IMediator _mediator;

        public MemberController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("UserPhotos")]
        [ProducesResponseType(typeof(IEnumerable<PhotoDto>), (int)HttpStatusCode.OK)]
       
        public async Task<ActionResult> UserPhotos([FromQuery]QueryUserPhotoRequest input)
        {
            var res = await _mediator.Send(input);
                return Ok(res);
        }

    }
}
