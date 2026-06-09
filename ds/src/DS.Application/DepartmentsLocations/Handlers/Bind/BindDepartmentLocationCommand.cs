using DS.Application.Abstractions.Handlers;
using DS.Contracts.DepartmentsLocations.Bind;

namespace DS.Application.DepartmentsLocations.Handlers.Bind;

public record BindDepartmentLocationCommand(BindDepartmentLocationRequest request) : ICommand;
