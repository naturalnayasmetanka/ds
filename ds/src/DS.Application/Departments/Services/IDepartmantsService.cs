using DS.Contracts.Departments.Create;
using DS.Contracts.Departments.Update;

namespace DS.Application.Departments.Services;

public interface IDepartmantsService
{
    Task<Guid> CreateAsync(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken);
    Task<Guid> UpdateAsync(
        Guid id,
        UpdateDepartmentRequest request,
        CancellationToken cancellationToken);
}
