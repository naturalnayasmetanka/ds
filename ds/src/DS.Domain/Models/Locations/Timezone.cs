using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;

namespace DS.Domain.Models.Locations;

public record Timezone
{
    private Timezone(string Value)
    {
        this.Value = Value.Trim();
    }

    public string Value { get; }

    public static Result<Timezone, Errors> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Timezone, Errors>(Error.Validation("empty.Timezone", "Timezone cannot be null", "Timezone"));
        }

        if (value.Length > 50)
        {
            return Result.Failure<Timezone, Errors>(Error.Validation("length.Timezone", "Max length 50", "Timezone"));
        }

        if (!TimeZoneInfo.TryFindSystemTimeZoneById(value.Trim(), out var tz))
        {
            return Result.Failure<Timezone, Errors>(Error.Validation("invalid.Timezone", "Invalid Timeazone", "Timezone"));
        }

        return Result.Success<Timezone, Errors>(new Timezone(Value: value));
    }
}
