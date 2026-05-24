using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Departments;

public record Path
{
    private Path(string Value)
    {
        this.Value = Value.Trim();
    }

    public string Value { get; }

    public static Result<Path, string> Create(string parentPath, string slug)
    {
        if (string.IsNullOrEmpty(parentPath) || string.IsNullOrEmpty(slug))
        {
            return Result.Failure<Path, string>
                ($"{nameof(Path)} empty or null");
        }

        return Result.Success<Path, string>
            (new Path(Value: parentPath + "." + slug));
    }

    public static Result<Path, string> Create(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return Result.Failure<Path, string>
                ($"{nameof(Path)} empty or null");
        }

        return Result.Success<Path, string>
            (new Path(Value: slug));
    }
}
