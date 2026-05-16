using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Departments;

public record Path
{
    private Path(string Value)
    {
        this.Value = Value.Trim();
    }

    public string Value { get; }

    public static Result<Path, string> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result.Failure<Path, string>
                ($"{nameof(Path)} empty or null");
        }

        return Result.Success<Path, string>
            (new Path(Value: value));
    }
}
