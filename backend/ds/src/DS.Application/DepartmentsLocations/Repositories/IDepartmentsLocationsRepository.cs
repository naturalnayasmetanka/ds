using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;
using DS.Domain.Models.DepartmentsLocations;

namespace DS.Application.DepartmentsLocations.Repositories;

public interface IDepartmentsLocationsRepository
{
    Task<UnitResult<Error>> AddRangeAsync(
        List<DepartmentLocation> departmentLocation,
        CancellationToken cancellationToken = default);

    Task<Result<DepartmentLocation?>> GetByIdsAsync(
        DepartmentLocation departmentLocation,
        CancellationToken cancellationToken = default);

    Task<Result<DepartmentLocation>> BindAsync(
        DepartmentLocation departmentLocation,
        CancellationToken cancellationToken = default);

    UnitResult<Error> Unbind(
        DepartmentLocation departmentLocation,
        CancellationToken cancellationToken = default);
}
