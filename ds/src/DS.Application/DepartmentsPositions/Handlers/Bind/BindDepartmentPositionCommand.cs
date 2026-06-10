using DS.Application.Abstractions.Handlers;
using DS.Contracts.DepartmentsPositions.Bind;

namespace DS.Application.DepartmentsPositions.Handlers.Bind;

public record BindDepartmentPositionCommand(BindDepartmentPositionRequest request) : ICommand;
