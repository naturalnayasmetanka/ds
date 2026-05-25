namespace DS.Contracts.Departments.Update;

public record UpdateDepartmentRequest(
    string Name,
    string Slug);