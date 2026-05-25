using DS.Application.Locations.Repositories;
using DS.Domain.Models.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DS.Infrastructure.Database.Emplementations.Repository;

public class EfCoreLocationsRepository : ILocationsRepository
{
    private readonly DsDbContext _dbContext;
    private readonly ILogger<EfCoreLocationsRepository> _logger;

    public EfCoreLocationsRepository(DsDbContext dbContext, ILogger<EfCoreLocationsRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;
    }

    public async Task AddAsync(Location newLocation, CancellationToken cancellationToken)
    {
        await _dbContext.Locations.AddAsync(newLocation, cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving changes to the database");
        }

    }

    public async Task<bool> ExistsByNameAsync(Name name, CancellationToken cancellationToken)
    {
        return await _dbContext.Locations.AnyAsync(l => l.Name == name, cancellationToken);
    }

    public Task<bool> AllLocationsExistAsync(List<Guid> ids, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<Location?> GetByFieldAsync(Expression<Func<Location, bool>> predicate, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public Task<List<Location>> GetListByFieldAsync(Expression<Func<Location, bool>> predicate, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
