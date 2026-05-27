using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Locations;

public record Name
{
    public Name(string Value)
    {
        this.Value = Value;
    }

    public string Value { get; }

    public static Result<Name> Create(string value)
        => Result.Success<Name>(new Name(Value: value));
}
