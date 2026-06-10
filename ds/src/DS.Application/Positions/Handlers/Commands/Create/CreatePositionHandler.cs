using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Positions.Repositories;
using DS.Contracts.Positions.Create;
using DS.Domain.Exceptions;
using DS.Domain.Models;
using DS.Domain.Models.Positions;
using Microsoft.Extensions.Logging;

namespace DS.Application.Positions.Handlers.Commands.Create;

public class CreatePositionHandler : ICommandHandler<Guid, CreatePositionCommand>
{
    private readonly IPositionsRepository _positionsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<CreatePositionHandler> _logger;

    public CreatePositionHandler(
        IPositionsRepository positionsRepository,
        ITransactionManager transactionManager,
        ILogger<CreatePositionHandler> logger)
    {
        _positionsRepository = positionsRepository;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<Result<Guid, Errors>> Handle(
        CreatePositionCommand command,
        CancellationToken cancellationToken = default)
    {
        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(cancellationToken);

        if (transactionScopeResult.IsFailure)
            return Result.Failure<Guid, Errors>(Error.Failure("transaction.failure", "Ошибка транзакции"));

        using var transactionScope = transactionScopeResult.Value;

        var newPosition = Position.Create(
            command.request.Name,
            command.request.Description).Value;

        var createPositionResult = await _positionsRepository
            .AddAsync(newPosition, cancellationToken);

        var saveResult = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (saveResult.IsFailure)
            return Result.Failure<Guid, Errors>(Error.Failure("save.failure", "Ошибка сохранения"));

        return Result.Success<Guid, Errors>(createPositionResult.Value);
    }
}
