using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Departments;

public record Identifier
{
    private Identifier(string Value)
    {
        this.Value = Value.Trim();
    }

    public string Value { get; }

    public static Result<Identifier> Create(string value) 
        => Result.Success<Identifier>(new Identifier(Value: value));
}
