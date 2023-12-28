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
public class CurrentUserBankAccountQuery : IRequest<BankAccount>
{
    public Guid UserId { get; set; }
}

public class CurrentUserBankAccountQueryHandler : IRequestHandler<CurrentUserBankAccountQuery, BankAccount>
{
    private readonly IApplicationDbContext _context;

    public CurrentUserBankAccountQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BankAccount> Handle(CurrentUserBankAccountQuery request, CancellationToken cancellationToken)
    {
        var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(x => x.UserId == request.UserId);
        return bankAccount;
    }
}