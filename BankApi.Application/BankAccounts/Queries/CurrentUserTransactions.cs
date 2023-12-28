using BankApi.Application.Common.Interfaces;
using BankApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Application.BankAccounts.Queries;
public class GetCurrentUserTransactionsQuery : IRequest<List<Transaction>>
{
    public Guid UserId { get; set; }
}

public class GetCurrentUserTransactionsQueryHandler : IRequestHandler<GetCurrentUserTransactionsQuery, List<Transaction>>
{
    private readonly IApplicationDbContext _context;

    public GetCurrentUserTransactionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> Handle(GetCurrentUserTransactionsQuery request, CancellationToken cancellationToken)
    {
        var bankAccount = await _context.BankAccounts.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == request.UserId);

        var bankAccountId = bankAccount.BankAccountId;

        var transactions = await _context.Transactions.AsNoTracking().Where(x => x.FromId == bankAccountId 
                                                              || x.ToId == bankAccountId).ToListAsync(cancellationToken);
        return transactions;
    }
}