using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;
using DS.Domain.Models.Locations;
using System.Linq.Expressions;

namespace DS.Application.Locations.Repositories;

public interface ILocationsRepository
{
    Task<Result<Guid>> AddAsync(
        Location newLocation,
        CancellationToken cancellationToken = default);
  
    Task<Result<bool>> AllLocationsExistAsync(
        List<Guid> ids,
        CancellationToken cancellationToken = default);

    Task<Result<Location?>> GetByFieldAsync(
    Expression<Func<Location, bool>> predicate,
    CancellationToken cancellationToken = default);

    Task<Result<List<Location>>> GetListByFieldAsync(
        Expression<Func<Location, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> SaveAsync(
        CancellationToken cancellationToken = default);
}
