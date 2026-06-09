using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Extentions;
using DS.Application.Locations.Repositories;
using DS.Contracts.Locations.Create;
using DS.Domain.Exceptions;
using DS.Domain.Models;
using DS.Domain.Models.Locations;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DS.Application.Locations.Handlers.Create;

public class CreateLocationHandler : ICommandHandler<Guid, CreateLocationCommand>
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<CreateLocationRequest> _createLocationRequestValidator;

    private readonly ILogger<CreateLocationHandler> _logger;

    public CreateLocationHandler(
        ILocationsRepository locationsRepository,
        IValidator<CreateLocationRequest> createLocationRequestValidator,
        ITransactionManager transactionManager,
        ILogger<CreateLocationHandler> logger)
    {
        _locationsRepository = locationsRepository;
        _createLocationRequestValidator = createLocationRequestValidator;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<Result<Guid, Errors>> Handle(
        CreateLocationCommand command,
        CancellationToken cancellationToken = default)
    {
        var fullValidationResult = await _createLocationRequestValidator
          .ValidateAsync(command.request, cancellationToken);

        if (!fullValidationResult.IsValid)
            return Result.Failure<Guid, Errors>(fullValidationResult.ToErrorList());

        var isAlreadyExistsByName =
            await _locationsRepository.GetByFieldAsync(x => x.Name == Name.Create(command.request.Name).Value, cancellationToken);

        if (isAlreadyExistsByName.Value is not null)
            return Result.Failure<Guid, Errors>(Error.Failure("location.already.exists", "Локация с таким именем уже существует"));

        var newLocation = Location.Create(
            Id.Create().Value,
            Name.Create(command.request.Name).Value,
            Address.Create(
                command.request.Adress.Country,
                command.request.Adress.Region,
                command.request.Adress.SettlementName,
                command.request.Adress.SettlementType,
                command.request.Adress.Street,
                command.request.Adress.BuildingNumber,
                command.request.Adress.BuildingBlock,
                command.request.Adress.Entrance,
                command.request.Adress.Floor,
                command.request.Adress.PremiseNumber,
                command.request.Adress.PremiseType,
                command.request.Adress.PostCode,
                command.request.Adress.FullAddress,
                command.request.Adress.Comment).Value,
            Timezone.Create(command.request.TimeZone).Value);

        await _locationsRepository
            .AddAsync(newLocation.Value, cancellationToken);

        var saveResult = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (saveResult.IsFailure)
            return Result.Failure<Guid, Errors>(Error.Failure("save.failure", "Ошибка сохранения"));

        return Result.Success<Guid, Errors>(newLocation.Value.Id);
    }
}