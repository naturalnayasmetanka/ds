using CSharpFunctionalExtensions;

namespace DS.Domain.Models.Locations;

public record Timezone
{
    private Timezone(string Value)
    {
        this.Value = Value.Trim();
    }

    public string Value { get; }

    public static Result<Timezone> Create(string value)
        => Result.Success<Timezone>(new Timezone(Value: value));
}
