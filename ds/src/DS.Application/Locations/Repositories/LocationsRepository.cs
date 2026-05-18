using DS.Domain.Models.Locations;

namespace DS.Application.Locations.Repositories;

public class LocationsRepository : ILocationsRepository
{
    public Task AddAsync(Location newLocation) => throw new NotImplementedException();
    public Task<bool> ExistsByNameAsync(Name name) => throw new NotImplementedException();
}
