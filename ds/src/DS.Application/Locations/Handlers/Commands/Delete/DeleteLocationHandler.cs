using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Locations.Repositories;
using DS.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace DS.Application.Locations.Handlers.Commands.Delete;

public class DeleteLocationHandler : ICommandHandler<Guid, DeleteLocationCommand>
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<DeleteLocationHandler> _logger;

    public DeleteLocationHandler(
        ILocationsRepository locationsRepository,
        ITransactionManager transactionManager,
        ILogger<DeleteLocationHandler> logger)
    {
        _locationsRepository = locationsRepository;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<Result<Guid, Errors>> Handle(
        DeleteLocationCommand command,
        CancellationToken cancellationToken = default)
    {
        var getLocationResult = await _locationsRepository
            .GetByFieldAsync(x => x.Id == command.Id && x.IsActive, cancellationToken);

        if (getLocationResult.Value is null)
            return Result.Failure<Guid, Errors>(Error.Failure("location.not.found", "Локация не найдена"));

        var location = getLocationResult.Value;
        location.Deactivate();

        var saveResult = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (saveResult.IsFailure)
            return Result.Failure<Guid, Errors>(Error.Failure("save.failure", "Ошибка сохранения"));

        return Result.Success<Guid, Errors>(location.Id);
    }
}
