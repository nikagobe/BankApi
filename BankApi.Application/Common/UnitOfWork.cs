using BankApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Application.Common;
public class UnitOfWork : IDisposable
{
    private readonly IDbContextTransaction _transaction;
    private readonly IApplicationDbContext _dbContext;
    
    public IApplicationDbContext DbContext => _dbContext;
    public UnitOfWork(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _transaction = _dbContext.StartTransaction();
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            _transaction.Rollback();
            throw;
        }
        finally
        {
            _transaction.Dispose();
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}
