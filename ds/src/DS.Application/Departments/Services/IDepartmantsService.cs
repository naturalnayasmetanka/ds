using DS.Contracts.Department.Create;
using DS.Contracts.Department.Update;
using DS.Domain.Models.Departments;

namespace DS.Application.Departments.Services;

public interface IDepartmantsService
{
    Task<Guid?> CreateAsync(
        CreateDepartmentRequest request, 
        CancellationToken cancellationToken);
    Task<Guid?> UpdateAsync(
        Guid id,
        UpdateDepartmentRequest request, 
        CancellationToken cancellationToken);
}
