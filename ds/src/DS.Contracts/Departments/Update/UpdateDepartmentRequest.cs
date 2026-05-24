namespace DS.Contracts.Department.Update;

public record UpdateDepartmentRequest(
    string Name,
    string Slug);