using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Shared.DAL.Postgres.Internals;

public sealed class PostgresUnitOfWork<T> : IUnitOfWork where T : DbContext
{
    private readonly T _dbContext;
    private readonly ILogger<PostgresUnitOfWork<T>> _logger; 

    public PostgresUnitOfWork(T dbContext, ILogger<PostgresUnitOfWork<T>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        _logger.LogInformation($"Database Transaction {transaction.TransactionId} started");
        
        try
        {
            await action();
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            _logger.LogInformation($"Database Transaction {transaction.TransactionId} rolling back");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }
}