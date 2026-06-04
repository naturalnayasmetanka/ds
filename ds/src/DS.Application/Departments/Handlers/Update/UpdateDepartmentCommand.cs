using DS.Application.Abstractions;
using DS.Contracts.Departments.Update;

namespace DS.Application.Departments.Handlers.Update;

public record UpdateDepartmentCommand(Guid departmentId, UpdateDepartmentRequest request) : ICommand;
