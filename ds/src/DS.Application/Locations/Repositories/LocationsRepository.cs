using DS.Domain.Models.Locations;
using DS.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DS.Application.Locations.Repositories;

public class LocationsRepository : ILocationsRepository
{
    private readonly DsDbContext _dbContext;
    public LocationsRepository(DsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(
        Location newLocation, 
        CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<bool> ExistsByNameAsync(
        Name name, 
        CancellationToken cancellationToken) => throw new NotImplementedException();

    public async Task<bool> AllLocationsExistAsync(
        List<Guid> ids, 
        CancellationToken cancellationToken)
    {
        var existingCount = 
            await _dbContext.Locations.Where(l => ids.Contains(l.Id))
            .CountAsync(cancellationToken);

        return ids.Count == existingCount;
    }

    public Guid UpdateLocation(
        Location location, 
        CancellationToken cancellationToken)
    {
        _dbContext.Attach(location);

        return location.Id;
    }

    public async Task SaveAsync(
        CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Location?> GetByIdAsync(
        Guid? id, 
        CancellationToken cancellationToken)
    {
        return id is null ? null : await _dbContext.Locations.FindAsync(id);
    }
}
