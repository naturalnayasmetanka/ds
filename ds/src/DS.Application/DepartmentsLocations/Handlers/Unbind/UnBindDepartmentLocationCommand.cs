using DS.Application.Abstractions;
using DS.Contracts.DepartmentsLocations.Unbind;

namespace DS.Application.DepartmentsLocations.Handlers.Unbind;

public record UnBindDepartmentLocationCommand(UnbindDepartmentLocationRequest request) : ICommand;
