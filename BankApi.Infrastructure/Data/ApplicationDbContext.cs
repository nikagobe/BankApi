using BankApi.Application.Common.Interfaces;
using BankApi.Application.Identity.Models.Entities;
using BankApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Infrastructure.Data;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<BankAccount> BankAccounts { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<Credentials> Credentials { get; set; }
    public void CommitTransaction()
    {
        Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        Database.RollbackTransaction();
    }

    public IDbContextTransaction StartTransaction()
    {
        return Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
    }
}
