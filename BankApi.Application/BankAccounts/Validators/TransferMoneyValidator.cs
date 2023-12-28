using BankApi.Application.BankAccounts.Commands;
using BankApi.Application.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Application.BankAccounts.Validators;
public class TransferMoneyValidator : AbstractValidator<TransferMoneyCommand>
{
    private readonly UnitOfWork _unitOfWork;

    public TransferMoneyValidator(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(v => v.FromAccountId )
           .MustAsync(async (FromAccountId, cancellation) => await ExistAccount(FromAccountId, cancellation))
               .WithMessage("Account You Are Trying To Transfer From Does Not Exits")
               .WithErrorCode("Invalid Operation");

        RuleFor(v => new { v.UserId, v.FromAccountId })
           .MustAsync( async (v, cancellation) => await OwnAccount(v.UserId, v.FromAccountId, cancellation))
               .WithMessage("Must Own Bank Account")
               .WithErrorCode("Unauthorised");

        RuleFor(v => new { v.FromAccountId, v.Amount})
           .MustAsync(async (v, cancellation) => await HaveEnoughMoney(v.FromAccountId, v.Amount, cancellation))
               .WithMessage("Must Have Enough Money")
               .WithErrorCode("Invalid Operation");

        RuleFor(v => v.ToAccountId )
           .MustAsync(async (ToAccountId, cancellation) => await ExistAccount(ToAccountId, cancellation))
               .WithMessage("Account You Are Trying To Transfer To Does Not Exits")
               .WithErrorCode("Invalid Operation");

        RuleFor(v => v.Amount)
           .GreaterThan(0)
               .WithMessage("Deposit Amount Must Be Greater Than Zero")
               .WithErrorCode("Invalid Operation");
    }

    public async Task<bool> ExistAccount(Guid AccountId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DbContext.BankAccounts.AnyAsync(x => x.BankAccountId == AccountId, cancellationToken);
    }

    public async Task<bool> OwnAccount(Guid UserId, Guid AccountId, CancellationToken cancellationToken)
    {
        var bankAccount = await _unitOfWork.DbContext.BankAccounts.FirstOrDefaultAsync(x => x.BankAccountId == AccountId, cancellationToken);

        return bankAccount.UserId == UserId;

    }

    public async Task<bool> HaveEnoughMoney(Guid AccountId, decimal Amount, CancellationToken cancellationToken)
    {
        var bankAccount = await _unitOfWork.DbContext.BankAccounts.FirstOrDefaultAsync(x => x.BankAccountId == AccountId, cancellationToken);

        return bankAccount.MoneyAmount >= Amount;

    }
}
