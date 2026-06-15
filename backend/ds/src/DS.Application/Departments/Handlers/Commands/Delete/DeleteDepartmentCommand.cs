using DS.Application.Abstractions.Handlers;

namespace DS.Application.Departments.Handlers.Commands.Delete;

public record DeleteDepartmentCommand(Guid Id) : ICommand;
