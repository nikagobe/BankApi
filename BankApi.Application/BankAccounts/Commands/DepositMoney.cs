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
public class DepositMoneyCommand : IRequest
{
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
}

public class DepositMoneyCommandHandler : IRequestHandler<DepositMoneyCommand>
{
    private readonly UnitOfWork _unitOfWork;

    public DepositMoneyCommandHandler(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DepositMoneyCommand request, CancellationToken cancellationToken)
    {
        var toAccount = await _unitOfWork.DbContext.BankAccounts.FirstAsync(x => x.BankAccountId == request.ToAccountId, cancellationToken);
        
        toAccount.MoneyAmount += request.Amount;

        var transaction = new Transaction()
        {
            Amount = request.Amount,
            ToId = request.ToAccountId,
            Type = Domain.Enums.TransactionType.Deposit
        };

        await _unitOfWork.DbContext.Transactions.AddAsync(transaction, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

    }
}