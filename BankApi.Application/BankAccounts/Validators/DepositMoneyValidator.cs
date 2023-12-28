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
public class DepositMoneyValidator : AbstractValidator<DepositMoneyCommand>
{
    private readonly UnitOfWork _unitOfWork;

    public DepositMoneyValidator(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(v => v.ToAccountId)
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
}
