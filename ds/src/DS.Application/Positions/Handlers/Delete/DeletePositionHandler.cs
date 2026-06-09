using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Positions.Repositories;
using DS.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace DS.Application.Positions.Handlers.Delete;

public class DeletePositionHandler : ICommandHandler<Guid, DeletePositionCommand>
{
    private readonly IPositionsRepository _positionsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<DeletePositionHandler> _logger;

    public DeletePositionHandler(
        IPositionsRepository positionsRepository,
        ITransactionManager transactionManager,
        ILogger<DeletePositionHandler> logger)
    {
        _positionsRepository = positionsRepository;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<Result<Guid, Errors>> Handle(
        DeletePositionCommand command,
        CancellationToken cancellationToken = default)
    {
        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(cancellationToken);

        if (transactionScopeResult.IsFailure)
            return Result.Failure<Guid, Errors>(Error.Failure("transaction.failure", "Ошибка транзакции"));

        using var transactionScope = transactionScopeResult.Value;

        var getPositionResult = await _positionsRepository
            .GetByFieldAsync(x => x.Id == command.Id, cancellationToken);

        if (getPositionResult.Value is null)
            return Result.Failure<Guid, Errors>(Error.Failure("position.not.found", "Должность не найдена"));

        var position = getPositionResult.Value;
        position.Deactivate();

        var saveResult = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (saveResult.IsFailure)
            return Result.Failure<Guid, Errors>(Error.Failure("save.failure", "Ошибка сохранения"));

        return Result.Success<Guid, Errors>(position.Id);
    }
}
