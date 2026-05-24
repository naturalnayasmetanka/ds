using DS.Domain.Models.Locations;

namespace DS.Application.Locations.Repositories;

public interface ILocationsRepository
{
    Task AddAsync(Location newLocation, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(Name name, CancellationToken cancellationToken);
    Task<bool> AllLocationsExistAsync(List<Guid> ids, CancellationToken cancellationToken);
    Guid UpdateLocation(Location location, CancellationToken cancellationToken);
    Task<Location?> GetByIdAsync(Guid? id, CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
}
