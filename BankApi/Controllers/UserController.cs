using BankApi.Application.BankAccounts.Queries;
using BankApi.Application.Identity.Service;
using BankApi.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : BaseController
{
    public UserController(IMediator mediator,
                          IIdentityService identityService) : base(mediator, identityService)
    {
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> User(CreateUserCommand request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> CurrentUserProfile()
    {

        var request = new GetCurrentUserProfileQuery()
        {
            UserId = await GetCurrentUserId()
        };

        return Ok(await _mediator.Send(request));
    }
}
