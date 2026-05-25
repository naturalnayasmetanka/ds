using DS.Domain.Models.Departments;
using System.Linq.Expressions;

namespace DS.Application.Departments.Repositories;

public interface IDepartmentsRepository
{
    Task<Guid> AddAsync(
        Department request,
        CancellationToken cancellationToken);

    Task SaveAsync(CancellationToken cancellationToken);

    Task<Department?> GetByFieldAsync(
        Expression<Func<Department, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<List<Department>> GetListByFieldAsync(
        Expression<Func<Department, bool>> predicate,
        CancellationToken cancellationToken = default);
}
