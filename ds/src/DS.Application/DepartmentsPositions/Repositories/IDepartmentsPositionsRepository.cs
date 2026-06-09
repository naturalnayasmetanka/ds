using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;
using DS.Domain.Models.DepartmentsPositions;
using System.Linq.Expressions;

namespace DS.Application.DepartmentsPositions.Repositories;

public interface IDepartmentsPositionsRepository
{
    Task<Result<Guid>> AddAsync(
        DepartmentPosition newDepartmentPosition,
        CancellationToken cancellationToken = default);

    Task<Result<DepartmentPosition?>> GetByFieldAsync(
        Expression<Func<DepartmentPosition, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<Result<List<DepartmentPosition>>> GetListByFieldAsync(
        Expression<Func<DepartmentPosition, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<Result<bool>> RemoveAsync(
        DepartmentPosition departmentPosition,
        CancellationToken cancellationToken = default);
}
