using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Departments;

public record Path
{
    private Path(string Value)
    {
        this.Value = Value.Trim();
    }

    public string Value { get; }

    public static Result<Path> Create(string parentPath, string slug)
        => Result.Success<Path>(new Path(Value: parentPath + "." + slug));

    public static Result<Path> Create(string slug)
        => Result.Success<Path>(new Path(Value: slug));
}
