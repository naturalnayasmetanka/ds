using CSharpFunctionalExtensions;
using DS.Application.Extentions;
using DS.Application.Locations.Repositories;
using DS.Contracts.Locations.Create;
using DS.Contracts.Locations.Update;
using DS.Domain.Exceptions;
using DS.Domain.Models;
using DS.Domain.Models.Locations;
using FluentValidation;

namespace DS.Application.Locations.Services;

public class LocationsService : ILocationsService
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly IValidator<CreateLocationRequest> _createLocationRequestValidator;
    private readonly IValidator<UpdateLocationRequest> _updateLocationRequetValidator;

    public LocationsService(
        ILocationsRepository locationsRepository,
        IValidator<CreateLocationRequest> createLocationRequestValidator,
        IValidator<UpdateLocationRequest> updateLocationRequetValidator)
    {
        _locationsRepository = locationsRepository;
        _createLocationRequestValidator = createLocationRequestValidator;
        _updateLocationRequetValidator = updateLocationRequetValidator;
    }

    public async Task<Result<Guid, Errors>> CreateAsync(
        CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var fullValidationResult = await _createLocationRequestValidator
            .ValidateAsync(request, cancellationToken);

        if (!fullValidationResult.IsValid)
            return Result.Failure<Guid, Errors>(fullValidationResult.ToErrorList());

        var isAlewadyExistsByName =
            await _locationsRepository.GetByFieldAsync(x => x.Name == Name.Create(request.Name).Value, cancellationToken);

        if (isAlewadyExistsByName.Value is not null)
            return Result.Failure<Guid, Errors>(Error.Failure("location.already.exists", "Локация с таким именем уже существует"));

        var newLocation = Location.Create(
            Id.Create().Value,
            Name.Create(request.Name).Value,
            Address.Create(
                request.Adress.Country,
                request.Adress.Region,
                request.Adress.SettlementName,
                request.Adress.SettlementType,
                request.Adress.Street,
                request.Adress.BuildingNumber,
                request.Adress.BuildingBlock,
                request.Adress.Entrance,
                request.Adress.Floor,
                request.Adress.PremiseNumber,
                request.Adress.PremiseType,
                request.Adress.PostCode,
                request.Adress.FullAddress,
                request.Adress.Comment).Value,
            Timezone.Create(request.TimeZone).Value);

        await _locationsRepository
            .AddAsync(newLocation.Value, cancellationToken);

        await _locationsRepository.SaveAsync(cancellationToken);

        return Result.Success<Guid, Errors>(newLocation.Value.Id);
    }

    public async Task<Result<Guid?, Errors>> UpdateAsync(
        Guid locationId,
        UpdateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var fullValidationResult = await _updateLocationRequetValidator
           .ValidateAsync(request, cancellationToken);

        if (!fullValidationResult.IsValid)
            return Result.Failure<Guid?, Errors>(fullValidationResult.ToErrorList());

        var existLocation = await _locationsRepository
          .GetByFieldAsync(x => x.Id == locationId, cancellationToken);

        if (existLocation.Value is null)
            return Result.Failure<Guid?, Errors>(Error.Failure("location.not.found", "Локация не найдена"));

        var updateLocation = Location.Update(
            existLocation.Value,
            Name.Create(request.Name).Value,
            Address.Create(
                request.Adress.Country,
                request.Adress.Region,
                request.Adress.SettlementName,
                request.Adress.SettlementType,
                request.Adress.Street,
                request.Adress.BuildingNumber,
                request.Adress.BuildingBlock,
                request.Adress.Entrance,
                request.Adress.Floor,
                request.Adress.PremiseNumber,
                request.Adress.PremiseType,
                request.Adress.PostCode,
                request.Adress.FullAddress,
                request.Adress.Comment).Value,
            Timezone.Create(request.TimeZone).Value);

        await _locationsRepository.SaveAsync(cancellationToken);

        return Result.Success<Guid?, Errors>(updateLocation.Value.Id);
    }
}
