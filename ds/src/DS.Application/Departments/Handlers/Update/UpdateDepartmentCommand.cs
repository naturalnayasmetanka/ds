using DS.Application.Abstractions.Handlers;
using DS.Contracts.Departments.Update;

namespace DS.Application.Departments.Handlers.Update;

public record UpdateDepartmentCommand(Guid departmentId, UpdateDepartmentRequest request) : ICommand;
