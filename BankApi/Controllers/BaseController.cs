using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers;
public abstract class BaseController : ControllerBase
{
    public IMediator _mediator { get; set; }

    public BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
