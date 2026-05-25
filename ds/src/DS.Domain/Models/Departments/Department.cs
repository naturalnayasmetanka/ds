using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Departments;

public class Department
{
    private List<Department> _childrens = [];

    //ef
    private Department() { }

    private Department(
        Guid id,
        Name name,
        Path path,
        Identifier identifier,
        Guid? parentId,
        int depth,
        int childrenCount)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
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
    public Department Parent { get; private set; }

    public IReadOnlyList<Department> Childrens => _childrens;

    public static Result<Department, string> Create(
        Guid id,
        Name name,
        Path path,
        Identifier identifer,
        Guid? parentId,
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
                id,
                name,
                path,
                identifer,
                parentId,
                depth,
                childrenCount));
    }

    public static Result<Department, string> Update(
        Department oldDepartment,
        Name name,
        Identifier slug)
    {
        return Result.Success<Department, string>(
            new Department(
                oldDepartment.Id,
                name,
                oldDepartment.Path,
                slug,
                oldDepartment.ParentId,
                oldDepartment.Depth,
                oldDepartment.ChildrenCount));
    }
}
