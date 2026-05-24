using DS.Application.Locations.Repositories;
using DS.Contracts.Location.Create;
using DS.Contracts.Location.Update;
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

    public async Task<Guid> CreateAsync(
        CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var fullValidationResult =
            await _createLocationRequestValidator
            .ValidateAsync(request, cancellationToken);

        if (!fullValidationResult.IsValid)
            throw new ValidationException(fullValidationResult.Errors);

        var isAlewadyExistsByName =
            await _locationsRepository
            .ExistsByNameAsync(Name.Create(request.Name).Value, cancellationToken);

        if (isAlewadyExistsByName)
            throw new Exception("Name already exists");

        var newLocation = Location.Create(
            Id.Create(),
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

        return await Task.FromResult(newLocation.Value.Id);
    }

    public async Task<Guid?> UpdateAsync(
        Guid locationId,
        UpdateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var fullValidationResult =
           await _updateLocationRequetValidator
           .ValidateAsync(request, cancellationToken);

        if (!fullValidationResult.IsValid)
            throw new ValidationException(fullValidationResult.Errors);

        var existLocation =
          await _locationsRepository
          .GetByIdAsync(locationId, cancellationToken);

        if (existLocation is null)
            return null;

        var updateLocation = Location.Update(
            existLocation,
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

        var updateLocationResult =
                _locationsRepository
            .UpdateLocation(updateLocation.Value, cancellationToken);

        await _locationsRepository.SaveAsync(cancellationToken);

        return updateLocation.Value.Id;
    }
}
