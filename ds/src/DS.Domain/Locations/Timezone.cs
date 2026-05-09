using CSharpFunctionalExtensions;
using TimeZoneConverter;

namespace DS.Domain.Locations;

public record Timezone
{
    private Timezone(string Value)
    {
        this.Value = Value.Trim();
    }

    public string Value { get; }

    public static Result<Timezone, string> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Timezone, string>
                ($"{nameof(Timezone)} empty or null");
        }

        if (!TZConvert.KnownIanaTimeZoneNames.Contains(value))
        {
            return Result.Failure<Timezone, string>
                ($"{nameof(Timezone)} isn`t IANA timezone");
        }

        return new Timezone(Value: value);
    }
}
