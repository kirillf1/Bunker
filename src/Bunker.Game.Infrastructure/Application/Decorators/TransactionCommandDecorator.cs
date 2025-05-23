﻿using Bunker.Game.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bunker.Game.Infrastructure.Application.Decorators;

public class TransactionCommandDecorator<TCommand, TCommandResult> : ICommandHandler<TCommand, TCommandResult>
{
    private readonly ICommandHandler<TCommand, TCommandResult> _decorated;
    private readonly BunkerGameDbContext _dbContext;
    private readonly ILogger<TransactionCommandDecorator<TCommand, TCommandResult>> _logger;

    private readonly int _maxRetryCount = 3;
    private readonly TimeSpan _initialRetryDelay = TimeSpan.FromMilliseconds(10);

    public TransactionCommandDecorator(
        ICommandHandler<TCommand, TCommandResult> decorated,
        BunkerGameDbContext dbContext,
        ILogger<TransactionCommandDecorator<TCommand, TCommandResult>> logger
    )
    {
        _decorated = decorated;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken)
    {
        var retryCount = 0;
        var delay = _initialRetryDelay;

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (_dbContext.HasActiveTransaction)
                    return await _decorated.Handle(command, cancellationToken);

                var transaction = await _dbContext.BeginTransactionAsync();

                try
                {
                    var result = await _decorated.Handle(command, cancellationToken);

                    if (transaction != null)
                        await _dbContext.CommitTransactionAsync(transaction);

                    return result;
                }
                catch
                {
                    _dbContext.RollbackTransaction();
                    throw;
                }
            }
            catch (Exception ex) when (ShouldRetry(ex, retryCount))
            {
                retryCount++;
                _logger.LogWarning(
                    ex,
                    "Try again handle command with transaction {RetryCount} after error: {Error}",
                    retryCount,
                    ex.Message
                );

                await Task.Delay(delay, cancellationToken);
                delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 2);
            }
        }

        throw new OperationCanceledException();
    }

    private bool ShouldRetry(Exception ex, int retryCount)
    {
        if (retryCount >= _maxRetryCount)
            return false;

        return ex is DbUpdateConcurrencyException || ex is DbUpdateException || IsTransientException(ex);
    }

    private bool IsTransientException(Exception ex)
    {
        if (ex.InnerException is Npgsql.PostgresException pgEx)
        {
            // 40001 - serialization_failure
            // 40P01 - deadlock_detected
            // 55P03 - lock_not_available
            var transientErrorCodes = new[] { "40001", "40P01", "55P03" };
            return transientErrorCodes.Contains(pgEx.SqlState);
        }

        return false;
    }
}
