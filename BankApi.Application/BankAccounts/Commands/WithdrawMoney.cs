using BankApi.Application.Common;
using BankApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Application.BankAccounts.Commands;
public class WithdrawMoneyCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid FromAccountId { get; set; }
    public decimal Amount { get; set; }
}

public class WithdrawMoneyCommandHandler : IRequestHandler<WithdrawMoneyCommand>
{
    private readonly UnitOfWork _unitOfWork;

    public WithdrawMoneyCommandHandler(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(WithdrawMoneyCommand request, CancellationToken cancellationToken)
    {
        var fromAccount = await _unitOfWork.DbContext.BankAccounts.FirstAsync(x => x.BankAccountId == request.FromAccountId, cancellationToken);

        fromAccount.MoneyAmount -= request.Amount;

        var transaction = new Transaction()
        {
            Amount = request.Amount,
            FromId = request.FromAccountId,
            Type = Domain.Enums.TransactionType.Withdrawal
        };

        await _unitOfWork.DbContext.Transactions.AddAsync(transaction, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

    }
}