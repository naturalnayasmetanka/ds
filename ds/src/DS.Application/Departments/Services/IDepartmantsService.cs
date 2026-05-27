using CSharpFunctionalExtensions;
using DS.Contracts.Departments.Create;
using DS.Contracts.Departments.Update;
using DS.Domain.Exceptions;

namespace DS.Application.Departments.Services;

public interface IDepartmantsService
{
    Task<Result<Guid, List<Error>>> CreateAsync(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken = default);
    Task<Result<Guid, List<Error>>> UpdateAsync(
        Guid id,
        UpdateDepartmentRequest request,
        CancellationToken cancellationToken = default);
}
