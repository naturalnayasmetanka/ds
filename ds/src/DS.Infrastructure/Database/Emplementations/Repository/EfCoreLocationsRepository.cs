using DS.Application.Locations.Repositories;
using DS.Domain.Models.Locations;
using DS.Infrastructure.Database.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
}
