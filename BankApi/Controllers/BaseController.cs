using BankApi.Application.Identity.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers;
public abstract class BaseController : ControllerBase
{
    protected IMediator _mediator { get; set; }
    protected IIdentityService _identityService { get; set; }

    public BaseController(IMediator mediator, 
                            IIdentityService identityService)
    {
        _mediator = mediator;
        _identityService = identityService;
    }

    protected async Task<Guid> GetCurrentUserId()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        var jwtToken = authorizationHeader.Substring("Bearer ".Length).Trim();


        var currentUserId = await _identityService.GetUserIdFromToken(jwtToken);

        return currentUserId;
    }
}
