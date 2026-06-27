using CSharpFunctionalExtensions;
using FS.Core.Exceptions;

namespace FS.Core.ValueObjects;

public sealed record StorageKey
{
    public StorageKey(
        string location,
        string prefix,
        string key)
    {
        Key = key;
        Prefix = prefix;
        Location = location;
        Value = string.IsNullOrEmpty(Prefix) ? Key : $"{Prefix}/{Key}";
        FullPath = string.IsNullOrEmpty(Prefix) ? Key : $"{Location}/{Prefix}/{Key}";
    }

    public string Key { get; }
    public string Prefix { get; }
    public string Location { get; }
    public string Value { get; }
    public string FullPath { get; }

    public static Result<StorageKey, Error> Create(string location, string? prefix, string key)
    {
        if (string.IsNullOrEmpty(location))
            return Result.Failure<StorageKey, Error>(Error.Failure("empty.location", "connot be empty"));

        var normalizedPrefixRezult = NormalizeSegment(prefix);
        if (normalizedPrefixRezult.IsFailure)
            return Result.Failure<StorageKey, Error>(Error.Failure("unnormalized.prefix", "unnormalized.prefix"));

        var normalizedKeyRezult = NormalizeSegment(key);
        if (normalizedKeyRezult.IsFailure)
            return Result.Failure<StorageKey, Error>(Error.Failure("unnormalized.key", "unnormalized.key"));

        return Result.Success<StorageKey, Error>
            (new StorageKey(location.Trim(), normalizedPrefixRezult.Value, normalizedKeyRezult.Value));
    }

    private static Result<string, Error> NormalizePrefix(string? prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            return string.Empty;

        string[] parts = prefix.Trim().Replace('\\', '/')
            .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        List<string> normalizedParts = [];

        foreach (string part in parts)
        {
            Result<string, Error> normalizedPart = NormalizeSegment(part);

            if (normalizedPart.IsFailure)
                return normalizedPart;

            if (!string.IsNullOrEmpty(normalizedPart.Value))
                normalizedParts.Add(normalizedPart.Value);
        }

        return string.Join("/", normalizedParts);
    }

    private static Result<string, Error> NormalizeSegment(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            return Result.Failure<string, Error>(Error.Failure("empty.value", "connot be empty"));

        string trimmed = value.Trim();

        if (trimmed.Contains('/', StringComparison.Ordinal) || trimmed.Contains('\\', StringComparison.Ordinal))
            return Result.Failure<string, Error>(Error.Failure("invalid.key", "invalid.key"));

        return trimmed;
    }
}