using DS.Domain.Models.Locations;

namespace DS.Application.Locations.Repositories;

public interface ILocationsRepository
{
    Task AddAsync(Location newLocation, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(Name name, CancellationToken cancellationToken);
}
