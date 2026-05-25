using DS.Contracts.Locations.Create;

namespace DS.Application.Locations.Services;

public interface ILocationsService
{
    Task<Guid> CreateLocationAsync(CreateLocationRequest request, CancellationToken cancellationToken = default);
}
