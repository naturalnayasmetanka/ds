namespace DS.Contracts.Departments.GetById;

public record GetDepartmentResponse()
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public int Depth { get; set; }
    public int ChildrenCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
