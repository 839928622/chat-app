using System.Net;
using Application.Features.Account.Commands.CreateNewUser;
using Application.Features.Account.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatHub.API.Controllers;

public class AccountController : BaseApiController
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisteredUserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Register(CreateNewUserCommand input)
    {
        var res = await _mediator.Send(input);
        if (res.Ok)
            return Ok(res.Data);
        return BadRequest(res.Msg);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserLoginSuccessReturnDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<UserLoginSuccessReturnDto>> Login(LoginRequest loginRequest)
    {
        var res = await _mediator.Send(loginRequest);
        if (!res.Ok)
            return Unauthorized(res.Msg);
        return Ok(res.Data);
    }
}