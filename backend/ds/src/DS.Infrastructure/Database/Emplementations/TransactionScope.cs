using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DS.Infrastructure.Database.Emplementations;

public class TransactionScope : ITransactionScope
{
    private readonly IDbTransaction _dbTransaction;
    private readonly ILogger<TransactionScope> _logger;

    public TransactionScope(
        IDbTransaction dbTransaction,
        ILogger<TransactionScope> logger)
    {
        _dbTransaction = dbTransaction;
        _logger = logger;
    }

    public UnitResult<Errors> Commit()
    {
        try
        {
            _dbTransaction.Commit();

            return UnitResult.Success<Errors>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "Failed to commit transaction");

            return UnitResult.Failure<Errors>(Error.Failure("transaction.commit.error", "Failed to commit transaction"));
        }
    }

    public UnitResult<Errors> Rollback()
    {
        try
        {
            _dbTransaction.Rollback();

            return UnitResult.Success<Errors>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "Failed to rollback transaction");

            return UnitResult.Failure<Errors>(Error.Failure("transaction.rollback.error", "Failed to rollback transaction"));
        }
    }

    public void Dispose()
    {
        _dbTransaction.Dispose();
    }
}
