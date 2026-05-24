using DS.Contracts.Location.Create;
using DS.Contracts.Location.Update;

namespace DS.Application.Locations.Services;

public interface ILocationsService
{
    Task<Guid> CreateAsync(
        CreateLocationRequest request,
        CancellationToken cancellationToken);

    Task<Guid?> UpdateAsync(
        Guid locationId,
        UpdateLocationRequest request,
        CancellationToken cancellationToken);
}
