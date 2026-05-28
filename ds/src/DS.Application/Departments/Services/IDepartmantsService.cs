using CSharpFunctionalExtensions;
using DS.Contracts.Departments.Create;
using DS.Contracts.Departments.Update;
using DS.Domain.Exceptions;

namespace DS.Application.Departments.Services;

public interface IDepartmantsService
{
    Task<Result<Guid, Errors>> CreateAsync(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken = default);
    Task<Result<Guid, Errors>> UpdateAsync(
        Guid id,
        UpdateDepartmentRequest request,
        CancellationToken cancellationToken = default);
}
