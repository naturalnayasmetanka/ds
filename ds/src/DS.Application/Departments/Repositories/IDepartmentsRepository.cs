using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;
using DS.Domain.Models.Departments;
using System.Linq.Expressions;

namespace DS.Application.Departments.Repositories;

public interface IDepartmentsRepository
{
    Task<Result<Guid>> AddAsync(
        Department request,
        CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> SaveAsync(
        CancellationToken cancellationToken = default);

    Task<Result<Department?>> GetByFieldAsync(
        Expression<Func<Department, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<Result<List<Department>>> GetListByFieldAsync(
        Expression<Func<Department, bool>> predicate,
        CancellationToken cancellationToken = default);
}
