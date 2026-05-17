using DS.Application.Locations.Repositories;
using DS.Contracts.Location.Create;
using DS.Domain.Models.Locations;
using FluentValidation;

namespace DS.Application.Locations.Services;

public class LocationsService : ILocationsService
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly IValidator<CreateLocationRequest> _createLocationRequestValidator;

    public LocationsService(
        ILocationsRepository locationsRepository,
        IValidator<CreateLocationRequest> createLocationRequestValidator)
    {
        _locationsRepository = locationsRepository;
        _createLocationRequestValidator = createLocationRequestValidator;
    }

    public async Task<Guid> CreateLocationAsync(
        CreateLocationRequest request, 
        CancellationToken cancellationToken)
    {
        if (await _locationsRepository.ExistsByNameAsync(Name.Create(request.Name).Value, cancellationToken))
            throw new Exception("Name already exists");

        var fullValidationResult = 
            await _createLocationRequestValidator.ValidateAsync(request, cancellationToken);

        if (!fullValidationResult.IsValid)
            throw new ValidationException(fullValidationResult.Errors);

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

        return await Task.FromResult(newLocation.Value.Id);
    }
}
