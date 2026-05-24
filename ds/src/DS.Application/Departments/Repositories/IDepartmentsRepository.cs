using DS.Domain.Models.Departments;

namespace DS.Application.Departments.Repositories;

public interface IDepartmentsRepository
{
    Task<Guid> CreateAsync(
        Department request,
        CancellationToken cancellationToken);

    Task<Department?> GetByIdAsync(
        Guid? id,
        CancellationToken cancellationToken);

    Guid Update(
        Department department,
        CancellationToken cancellationToken);

    Task SaveAsync(CancellationToken cancellationToken);
}
