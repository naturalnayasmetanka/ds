using DS.Application.Abstractions;
using DS.Contracts.DepartmentsLocations.Bind;

namespace DS.Application.DepartmentsLocations.Handlers.Bind;

public record BindDepartmentLocationCommand(BindDepartmentLocationRequest request) : ICommand;
