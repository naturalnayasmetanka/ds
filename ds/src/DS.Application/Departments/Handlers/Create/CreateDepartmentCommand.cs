using DS.Application.Abstractions;
using DS.Contracts.Departments.Create;

namespace DS.Application.Departments.Handlers.Create;

public record CreateDepartmentCommand(CreateDepartmentRequest request) : ICommand;
