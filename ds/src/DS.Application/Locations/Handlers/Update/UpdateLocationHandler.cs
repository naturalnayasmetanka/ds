using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Extentions;
using DS.Application.Locations.Repositories;
using DS.Contracts.Locations.Update;
using DS.Domain.Exceptions;
using DS.Domain.Models.Locations;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DS.Application.Locations.Handlers.Update;

public class UpdateLocationHandler : ICommandHandler<Guid, UpdateLocationCommand>
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<UpdateLocationRequest> _updateLocationRequetValidator;

    private readonly ILogger<UpdateLocationHandler> _logger;

    public UpdateLocationHandler(
        ILocationsRepository locationsRepository,
        ITransactionManager transactionManager,
        IValidator<UpdateLocationRequest> updateLocationRequetValidator,
        ILogger<UpdateLocationHandler> logger)
    {
        _locationsRepository = locationsRepository;
        _updateLocationRequetValidator = updateLocationRequetValidator;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<Result<Guid, Errors>> Handle(
        UpdateLocationCommand command,
        CancellationToken cancellationToken = default)
    {
        var fullValidationResult = await _updateLocationRequetValidator
         .ValidateAsync(command.request, cancellationToken);

        if (!fullValidationResult.IsValid)
            return Result.Failure<Guid, Errors>(fullValidationResult.ToErrorList());

        var existLocation = await _locationsRepository
          .GetByFieldAsync(x => x.Id == command.locationId, cancellationToken);

        if (existLocation.Value is null)
            return Result.Failure<Guid, Errors>(Error.Failure("location.not.found", "Локация не найдена"));

        var updateLocation = Location.Update(
            existLocation.Value,
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

        await _transactionManager.SaveChangesAsync(cancellationToken);

        return Result.Success<Guid, Errors>(updateLocation.Value.Id);
    }
}
