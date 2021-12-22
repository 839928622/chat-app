using Application.Features.Account.Commands.CreateNewUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatHub.API.Controllers;

public class AccountController : BaseApiController
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("register")]
    public async Task<ActionResult> Register(CreateNewUserCommand input)
    {
      var res=  await _mediator.Send(input);
      if (res.Ok)
       return Ok(res.Data);
      return BadRequest(res.Msg);
    }
}