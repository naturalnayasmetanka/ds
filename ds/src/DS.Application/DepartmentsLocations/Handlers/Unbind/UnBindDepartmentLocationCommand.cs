using DS.Application.Abstractions.Handlers;
using DS.Contracts.DepartmentsLocations.Unbind;

namespace DS.Application.DepartmentsLocations.Handlers.Unbind;

public record UnBindDepartmentLocationCommand(UnbindDepartmentLocationRequest request) : ICommand;
