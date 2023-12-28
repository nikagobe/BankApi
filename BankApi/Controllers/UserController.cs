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
    private readonly IIdentityService _identityService;

    public UserController(IMediator mediator,
                          IIdentityService identityService) : base(mediator)
    {
        _identityService = identityService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> User(CreateUserCommand request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpGet]
    public async Task<IActionResult> CurrentUserProfile()
    {

        var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        var jwtToken = authorizationHeader.Substring("Bearer ".Length).Trim();


        var currentUserId = await _identityService.GetUserIdFromToken(jwtToken);

        var request = new CurrentUserProfileQuery()
        {
            UserId = currentUserId
        };

        return Ok(await _mediator.Send(request));
    }
}
