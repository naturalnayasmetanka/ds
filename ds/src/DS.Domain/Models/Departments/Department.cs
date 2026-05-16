using CSharpFunctionalExtensions;
using DS.Domain.Models.DepartmentsLocations;
using DS.Domain.Models.DepartmentsPositions;

namespace DS.Domain.Models.Departments;

public class Department
{
    private List<Department> _childrens = [];
    private List<DepartmentPosition> _departmentsPositions = [];
    private List<DepartmentLocation> _departmentsLocations = [];

    //ef
    private Department() { }

    private Department(
        Name name,
        Path path,
        Identifier identifier,
        int depth,
        int childrenCount)
    {
        Id = Guid.NewGuid();
        Name = name;
        Identifier = identifier;
        Path = path;
        Depth = depth;
        ChildrenCount = childrenCount;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public Identifier Identifier { get; private set; }
    public Path Path { get; private set; }
    public Guid? ParentId { get; private set; }
    public int Depth { get; private set; }
    public int ChildrenCount { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<DepartmentPosition> DepartmentsPositions => _departmentsPositions;
    public IReadOnlyList<DepartmentLocation> DepartmentsLocations => _departmentsLocations;

    public Department Parent { get; private set; }

    public IReadOnlyList<Department> Childrens => _childrens;

    public static Result<Department, string> Create(
        Name name,
        Path path,
        Identifier identifer,
        int depth,
        int childrenCount)
    {
        if (depth < 0)
        {
            return Result.Failure<Department, string>
                ($"{nameof(Department)} {nameof(depth)} less 0");
        }

        if (childrenCount < 0)
        {
            return Result.Failure<Department, string>
                ($"{nameof(Department)} {nameof(childrenCount)} less 0");
        }

        return Result.Success<Department, string>(
            new Department(
                name,
                path,
                identifer,
                depth,
                childrenCount));
    }
}
