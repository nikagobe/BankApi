using BankApi.Application.BankAccounts.Commands;
using BankApi.Application.BankAccounts.Dto;
using BankApi.Application.BankAccounts.Queries;
using BankApi.Application.Identity.Service;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class BankAccountController : BaseController
{
    private readonly IIdentityService _identityService;
    public BankAccountController(IMediator mediator, 
                                 IIdentityService identityService) : base(mediator)
    {
        _identityService = identityService;
    }

    [HttpPost]
    [Route("Transfer")]
    public async Task<IActionResult> Transfer(TransferMoneyDto request)
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        var jwtToken = authorizationHeader.Substring("Bearer ".Length).Trim();


        var currentUserId = await _identityService.GetUserIdFromToken(jwtToken);

        var command = new TransferMoneyCommand()
        {
            Amount = request.Amount,
            FromAccountId = request.FromAccountId,
            ToAccountId = request.ToAccountId,
            UserId = currentUserId
        };


        await _mediator.Send(request);

        return Ok();
    }

    [HttpPost]
    [Route("Deposit")]
    [AllowAnonymous]
    public async Task<IActionResult> Deposit(DepositMoneyCommand request)
    {

        await _mediator.Send(request);

        return Ok();
    }

    [HttpPost]
    [Route("Withdraw")]
    public async Task<IActionResult> Withdraw(WithdrawMoneyCommand request)
    {

        var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        var jwtToken = authorizationHeader.Substring("Bearer ".Length).Trim();


        var currentUserId = await _identityService.GetUserIdFromToken(jwtToken);

        request.UserId = currentUserId;

        await _mediator.Send(request);

        return Ok();
    }

    [HttpGet]
    [Route("CurrentUser")]
    public async Task<IActionResult> CurrentUserBankAccount()
    {

        var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        var jwtToken = authorizationHeader.Substring("Bearer ".Length).Trim();


        var currentUserId = await _identityService.GetUserIdFromToken(jwtToken);

        var request = new CurrentUserBankAccountQuery()
        {
            UserId = currentUserId
        };

        return Ok(await _mediator.Send(request));
    }

    [HttpGet]
    [Route("CurrentUser/Transactions")]
    public async Task<IActionResult> CurrentUserTransactions()
    {

        var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        var jwtToken = authorizationHeader.Substring("Bearer ".Length).Trim();


        var currentUserId = await _identityService.GetUserIdFromToken(jwtToken);

        var request = new CurrentUserTransactionsQuery()
        {
            UserId = currentUserId
        };

        return Ok(await _mediator.Send(request));
    }
}