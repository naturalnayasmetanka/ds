using DS.Domain.Models.Locations;
using DS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DS.Application.Locations.Repositories;

public class LocationsRepository : ILocationsRepository
{
    private readonly DsDbContext _dbContext;
    private readonly ILogger<LocationsRepository> _logger;
    public LocationsRepository(
        DsDbContext dbContext,
        ILogger<LocationsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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

    public async Task SaveAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public async Task<Location?> GetByFieldAsync(
        Expression<Func<Location, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Location>().FirstOrDefaultAsync(predicate, cancellationToken);
    }
    public async Task<List<Location>> GetListByFieldAsync(
        Expression<Func<Location, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Location>().Where(predicate).ToListAsync(cancellationToken);
    }
}
