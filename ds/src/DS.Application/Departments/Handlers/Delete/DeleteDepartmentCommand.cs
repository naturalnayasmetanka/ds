using DS.Application.Abstractions.Handlers;

namespace DS.Application.Departments.Handlers.Delete;

public record DeleteDepartmentCommand(Guid Id) : ICommand;
