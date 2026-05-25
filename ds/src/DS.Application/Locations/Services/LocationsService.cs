using DS.Application.Locations.Repositories;
using DS.Contracts.Locations.Create;
using DS.Domain.Models.Locations;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DS.Application.Locations.Services;

public class LocationsService : ILocationsService
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly ILogger<LocationsService> _logger;
    private readonly IValidator<CreateLocationRequest> _createLocationRequestValidator;

    public LocationsService(
        ILocationsRepository locationsRepository,
        ILogger<LocationsService> logger,
        IValidator<CreateLocationRequest> createLocationRequestValidator)
    {
        _locationsRepository = locationsRepository;
        _logger = logger;
        _createLocationRequestValidator = createLocationRequestValidator;
    }

    public async Task<Guid> CreateLocationAsync(
        CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var fullValidationResult =
            await _createLocationRequestValidator.ValidateAsync(request, cancellationToken);

        if (!fullValidationResult.IsValid)
            throw new ValidationException(fullValidationResult.Errors);

        var isAlewadyExistsByName =
            await _locationsRepository.ExistsByNameAsync(Name.Create(request.Name).Value, cancellationToken);

        if (isAlewadyExistsByName)
            throw new Exception("Name already exists");

        var newLocation = Location.Create(
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

        await _locationsRepository.AddAsync(newLocation.Value, cancellationToken);

        await _locationsRepository.SaveAsync(cancellationToken);

        return await Task.FromResult(newLocation.Value.Id);
    }
}
