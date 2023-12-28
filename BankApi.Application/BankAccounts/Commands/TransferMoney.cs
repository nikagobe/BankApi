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
public class TransferMoneyCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
}

public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand>
{
    private readonly UnitOfWork _unitOfWork;

    public TransferMoneyCommandHandler(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
    {
        var fromAccount = await _unitOfWork.DbContext.BankAccounts.FirstAsync(x => x.BankAccountId == request.FromAccountId, cancellationToken);
        var toAccount = await _unitOfWork.DbContext.BankAccounts.FirstAsync(x => x.BankAccountId == request.ToAccountId, cancellationToken);

        fromAccount.MoneyAmount -= request.Amount;
        toAccount.MoneyAmount += request.Amount;

        var transaction = new Transaction()
        {
            Amount = request.Amount,
            FromId = request.FromAccountId,
            ToId = request.ToAccountId,
            Type = Domain.Enums.TransactionType.Transfer
        };

        await _unitOfWork.DbContext.Transactions.AddAsync(transaction, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

    }
}