using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using BankApi.Application.Identity.Models.Entities;

namespace BankApi.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<BankAccount> BankAccounts { get; }
    DbSet<Transaction> Transactions { get; }
    DbSet<Credentials> Credentials { get; }

    IDbContextTransaction StartTransaction();
    void RollbackTransaction();
    void CommitTransaction();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
