using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Locations;

public record Name
{
    public Name(string Value)
    {
        this.Value = Value;
    }

    public string Value { get; }

    public static Result<Name, string> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result.Failure<Name, string>
                ($"{nameof(Name)} empty or null");
        }

        return Result.Success<Name, string>
            (new Name(Value: value));
    }
}
