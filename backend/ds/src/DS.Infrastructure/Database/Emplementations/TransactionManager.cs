using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DS.Infrastructure.Database.Emplementations;

public class TransactionManager : ITransactionManager
{
    private readonly DsDbContext _dbContext;
    private readonly ILogger<TransactionManager> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public TransactionManager(
        DsDbContext dbContext,
        ILogger<TransactionManager> logger,
        ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    public async Task<Result<ITransactionScope, Errors>> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var logger = _loggerFactory.CreateLogger<TransactionScope>();
            var transactionScope = new TransactionScope(transaction.GetDbTransaction(), logger);

            return Result.Success<ITransactionScope, Errors>(transactionScope);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "Failed transaction");

            return Result.Failure<ITransactionScope, Errors>(Error.Failure("transaction.failure", $"{ex.Message} Failed transaction"));
        }
    }

    public async Task<UnitResult<Errors>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);

            return UnitResult.Success<Errors>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "Failed save changes");

            return UnitResult.Failure<Errors>(Error.Failure("save.failure", $"{ex.Message} Failed save changes"));
        }
    }
}
