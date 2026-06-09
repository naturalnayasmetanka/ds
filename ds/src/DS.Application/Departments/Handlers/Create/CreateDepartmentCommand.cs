using DS.Application.Abstractions.Handlers;
using DS.Contracts.Departments.Create;

namespace DS.Application.Departments.Handlers.Create;

public record CreateDepartmentCommand(CreateDepartmentRequest request) : ICommand;
