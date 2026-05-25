namespace DS.Contracts.Department.Create;

public record CreateDepartmentRequest(
    string Name,
    string Slug,
    Guid? ParentId,
    List<Guid> Locations);
