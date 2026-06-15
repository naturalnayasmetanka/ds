namespace DS.Contracts.DepartmentsPositions.Bind;

public record BindDepartmentPositionRequest(Guid DepartmentId, Guid PositionId);

public record BindDepartmentPositionResponse(bool Success);
