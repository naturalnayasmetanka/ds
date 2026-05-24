using DS.Domain.Models.DepartmentsLocations;

namespace DS.Application.DepartmentsLocations.Repositories;

public interface IDepartmentsLocationsRepository
{
    Task CreateRangeAsync(
        List<DepartmentLocation> departmentLocation,
        CancellationToken cancellationToken);

    Task<DepartmentLocation?> GetByIdsAsync(
        DepartmentLocation departmentLocation,
        CancellationToken cancellationToken);

    Task<DepartmentLocation> BindAsync(
        DepartmentLocation departmentLocation,
        CancellationToken cancellationToken);

    void UnbindAsync(
        DepartmentLocation departmentLocation,
        CancellationToken cancellationToken);

    Task SaveAsync(CancellationToken cancellationToken);
}
