using CSharpFunctionalExtensions;
using DS.Contracts.Locations.Create;
using DS.Contracts.Locations.Update;
using DS.Domain.Exceptions;

namespace DS.Application.Locations.Services;

public interface ILocationsService
{
    Task<Result<Guid, List<Error>>> CreateAsync(
        CreateLocationRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<Guid?, List<Error>>> UpdateAsync(
        Guid locationId,
        UpdateLocationRequest request,
        CancellationToken cancellationToken = default);
}
