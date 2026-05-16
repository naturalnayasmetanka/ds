using CSharpFunctionalExtensions;
using DS.Domain.Models.DepartmentsPositions;

namespace DS.Domain.Models.Positions;

public class Position
{
    private List<DepartmentPosition> _departmentsPositions = [];

    private Position(
        string name,
        string? description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = true;
        CreateAt = DateTime.UtcNow;
        UpdateAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreateAt { get; private set; }
    public DateTime UpdateAt { get; private set; }

    public IReadOnlyList<DepartmentPosition> DepartmentsPositions => _departmentsPositions;

    public static Result<Position, string> Create(
        string name,
        string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Position, string>
                ($"{nameof(Position)} empty or null");
        }

        return Result.Success<Position, string>
            (new Position(name, description));
    }
}
