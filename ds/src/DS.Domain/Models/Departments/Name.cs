using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Departments;

public record Name
{
    private Name(string Value)
    {
        this.Value = Value.Trim();
    }

    public string Value { get; }

    public static Result<Name> Create(string value)
    {
        return Result.Success<Name>(new Name(Value: value));
    }
}
