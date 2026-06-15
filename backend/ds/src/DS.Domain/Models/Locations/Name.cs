using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;

namespace DS.Domain.Models.Locations;

public record Name
{
    public Name(string Value)
    {
        this.Value = Value;
    }

    public string Value { get; }

    public static Result<Name, Errors> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Name, Errors>(Error.Validation("empty.name", "Name cannot be null", "Name"));
        }

        if (value.Length < 3)
        {
            return Result.Failure<Name, Errors>(Error.Validation("minlength.name", "Name min length 3", "Name"));
        }

        if (value.Length > 100)
        {
            return Result.Failure<Name, Errors>(Error.Validation("maxlength.name", "Name max length 100", "Name"));
        }

        return Result.Success<Name, Errors>(new Name(Value: value));
    }
}
