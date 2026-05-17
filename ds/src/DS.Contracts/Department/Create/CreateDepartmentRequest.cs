namespace DS.Contracts.Department.Create;

public record CreateDepartmentRequest(
    string Name,
    string Identifier,
    string Path,
    int Depth,
    int ChildrenCount);
