using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Departments;

public record Identifier
{
    private Identifier(string Value)
    {
        this.Value = Value.Trim();
    }

    public string Value { get; }

    public static Result<Identifier, string> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Identifier, string>
                ($"{nameof(Identifier)} empty or null");
        }

        return Result.Success<Identifier, string>
            (new Identifier(Value: value));
    }
}
