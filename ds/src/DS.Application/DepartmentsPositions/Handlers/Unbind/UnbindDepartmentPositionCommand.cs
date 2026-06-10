using DS.Application.Abstractions.Handlers;
using DS.Contracts.DepartmentsPositions.Unbind;

namespace DS.Application.DepartmentsPositions.Handlers.Unbind;

public record UnbindDepartmentPositionCommand(UnbindDepartmentPositionRequest request) : ICommand;
