using DS.Domain.Models.Locations;
using System.Linq.Expressions;

namespace DS.Application.Locations.Repositories;

public interface ILocationsRepository
{
    Task AddAsync(Location newLocation, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(Name name, CancellationToken cancellationToken);
    Task<bool> AllLocationsExistAsync(List<Guid> ids, CancellationToken cancellationToken);

    Task<Location?> GetByFieldAsync(
    Expression<Func<Location, bool>> predicate,
    CancellationToken cancellationToken = default);

    Task<List<Location>> GetListByFieldAsync(
        Expression<Func<Location, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task SaveAsync(CancellationToken cancellationToken);
}
