using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;
using DS.Domain.Models.Positions;
using System.Linq.Expressions;

namespace DS.Application.Positions.Repositories;

public interface IPositionsRepository
{
    Task<Result<Guid>> AddAsync(
        Position newPosition,
        CancellationToken cancellationToken = default);

    Task<Result<Position?>> GetByFieldAsync(
        Expression<Func<Position, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<Result<List<Position>>> GetListByFieldAsync(
        Expression<Func<Position, bool>> predicate,
        CancellationToken cancellationToken = default);
}
