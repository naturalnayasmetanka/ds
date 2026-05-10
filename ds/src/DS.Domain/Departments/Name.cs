using CSharpFunctionalExtensions;

namespace DS.Domain.Departments;

public record Name
{
    private const int MIN_LENGTH = 3;
    private const int MAX_LENGTH = 50;

    private Name(string Value)
    {
        this.Value = Value.Trim();
    }

    public string Value { get; }

    public static Result<Name, string> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Name, string>
                ($"{nameof(Name)} empty or null");
        }

        if (value.Length <= MIN_LENGTH)
        {
            return Result.Failure<Name, string>
                ($"{nameof(Name)} minlength {MIN_LENGTH}");
        }

        if (value.Length >= MIN_LENGTH)
        {
            return Result.Failure<Name, string>
                ($"{nameof(Name)} maxlength {MAX_LENGTH}");
        }

        return Result.Success<Name, string>
            (new Name(Value: value));
    }
}
