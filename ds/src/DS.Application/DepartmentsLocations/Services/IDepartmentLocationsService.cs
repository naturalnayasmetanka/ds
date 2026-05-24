using DS.Contracts.DepartmentsLocations.Bind;
using DS.Contracts.DepartmentsLocations.Unbind;

namespace DS.Application.DepartmentsLocations.Services;

public interface IDepartmentLocationsService
{
    Task<(Guid, Guid)?> BindAsync(
        BindDepartmentLocationRequest request,
        CancellationToken cancellation);

    Task<(Guid, Guid)?> UnbindAsync(
        UnbindDepartmentLocationRequest request,
        CancellationToken cancellation);
}
