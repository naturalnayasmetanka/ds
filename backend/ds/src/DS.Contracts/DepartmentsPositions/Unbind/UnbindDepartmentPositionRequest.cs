namespace DS.Contracts.DepartmentsPositions.Unbind;

public record UnbindDepartmentPositionRequest(Guid DepartmentId, Guid PositionId);

public record UnbindDepartmentPositionResponse(bool Success);
