namespace DS.Contracts.Department.Update;

public record UpdateDepartmentRequest(
    string Name,
    string Identifier,
    string Path,
    int Depth,
    int ChildrenCount);