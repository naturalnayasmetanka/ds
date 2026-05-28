using CSharpFunctionalExtensions;
using DS.Application.Locations.Repositories;
using DS.Domain.Exceptions;
using DS.Domain.Models.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DS.Infrastructure.Database.Emplementations.Repository;

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

    public async Task<Result<Guid>> AddAsync(Location newLocation, CancellationToken cancellationToken = default)
    {
        await _dbContext.Locations.AddAsync(newLocation, cancellationToken);

        return newLocation.Id;
    }

    public async Task<Result<bool>> AllLocationsExistAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        var existingCount = await _dbContext.Locations.Where(l => ids.Contains(l.Id)).CountAsync(cancellationToken);
        var result = ids.Count == existingCount;

        return Result.Success<bool>(result);
    }

    public async Task<UnitResult<Error>> SaveAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<Error>();
    }

    public async Task<Result<Location?>> GetByFieldAsync(Expression<Func<Location, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Locations.FirstOrDefaultAsync(predicate, cancellationToken);

        return Result.Success<Location?>(result);
    }
    public async Task<Result<List<Location>>> GetListByFieldAsync(Expression<Func<Location, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Locations.Where(predicate).ToListAsync(cancellationToken);

        return Result.Success<List<Location>>(result);
    }
}
