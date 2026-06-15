using DS.Application.Abstractions.Handlers;
using DS.Contracts.Departments.Create;

namespace DS.Application.Departments.Handlers.Commands.Create;

public record CreateDepartmentCommand(CreateDepartmentRequest request) : ICommand;
