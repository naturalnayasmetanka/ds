using CSharpFunctionalExtensions;
using DS.Contracts.DepartmentsLocations.Bind;
using DS.Contracts.DepartmentsLocations.Unbind;
using DS.Domain.Exceptions;

namespace DS.Application.DepartmentsLocations.Services;

public interface IDepartmentLocationsService
{
    Task<Result<(Guid, Guid)?, List<Error>>> BindAsync(
        BindDepartmentLocationRequest request,
        CancellationToken cancellation = default);

    Task<Result<(Guid, Guid)?, List<Error>>> UnbindAsync(
        UnbindDepartmentLocationRequest request,
        CancellationToken cancellation = default);
}
