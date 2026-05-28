using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;

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

    public static Result<Department, Errors> Create(Guid id, Name name, Path path, Identifier identifer, Guid? parentId, int depth, int childrenCount)
    {
        if (depth < 0)
        {
            return Result.Failure<Department, Errors>(Error.Validation("invalid.depth", "Глубина меньше нуля"));
        }

        if (childrenCount < 0)
        {
            return Result.Failure<Department, Errors>(Error.Validation("invalid.childrenCount", "Количество дочерних элементов меньше нуля"));
        }

        return Result.Success<Department, Errors>(new Department(id, name, path, identifer, parentId, depth, childrenCount));
    }


    public static Result<Department, Errors> Update(Department old, Name name, Identifier slug)
    {
        if (string.IsNullOrEmpty(name.Value))
        {
            return Result.Failure<Department, Errors>(Error.Validation("invalid.depth", "Невалидное имя"));
        }

        return Result.Success<Department, Errors>(new Department(old.Id, name, old.Path, slug, old.ParentId, old.Depth, old.ChildrenCount));
    }
}
