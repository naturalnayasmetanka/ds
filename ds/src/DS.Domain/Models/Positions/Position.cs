using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Positions;

public class Position
{
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

    public static Result<Position> Create(string name, string? description)
        => Result.Success<Position>(new Position(name, description));
}
