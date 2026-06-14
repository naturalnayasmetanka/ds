namespace DS.Contracts.Departments.Get;

public record DepartmentListItemDto(
    Guid Id,
    string Name,
    string Path,
    DateTime CreatedAt,
    DateTime UpdatedAt);
