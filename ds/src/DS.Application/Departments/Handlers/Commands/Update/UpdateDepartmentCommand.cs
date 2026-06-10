using DS.Application.Abstractions.Handlers;
using DS.Contracts.Departments.Update;

namespace DS.Application.Departments.Handlers.Commands.Update;

public record UpdateDepartmentCommand(Guid departmentId, UpdateDepartmentRequest request) : ICommand;
