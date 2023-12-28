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
    public BankAccountController(IMediator mediator, 
                                 IIdentityService identityService) : base(mediator, identityService)
    { 
    }

    [HttpPost]
    [Route("Transfer")]
    public async Task<IActionResult> Transfer(TransferMoneyDto request)
    {

        var command = new TransferMoneyCommand()
        {
            Amount = request.Amount,
            FromAccountId = request.FromAccountId,
            ToAccountId = request.ToAccountId,
            UserId = await GetCurrentUserId()
        };


        await _mediator.Send(command);

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

        request.UserId = await GetCurrentUserId();

        await _mediator.Send(request);

        return Ok();
    }

    [HttpGet]
    [Route("CurrentUser")]
    public async Task<IActionResult> CurrentUserBankAccount()
    {

        var request = new GetCurrentUserBankAccountQuery()
        {
            UserId = await GetCurrentUserId()
        };

        return Ok(await _mediator.Send(request));
    }

    [HttpGet]
    [Route("CurrentUser/Transactions")]
    public async Task<IActionResult> CurrentUserTransactions()
    {

        var request = new GetCurrentUserTransactionsQuery()
        {
            UserId = await GetCurrentUserId()
        };

        return Ok(await _mediator.Send(request));
    }
}