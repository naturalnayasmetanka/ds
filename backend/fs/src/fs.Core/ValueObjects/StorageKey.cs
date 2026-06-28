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
        FullPath = string.IsNullOrEmpty(Prefix) ? $"{Location}/{Key}" : $"{Location}/{Prefix}/{Key}";
    }

    public string Key { get; }
    public string Prefix { get; }
    public string Location { get; }
    public string Value { get; }
    public string FullPath { get; }

    public static Result<StorageKey, Error> Create(string location, string? prefix, string key)
    {
        if (string.IsNullOrEmpty(location))
            return Result.Failure<StorageKey, Error>(Error.Failure("empty.location", "cannot be empty"));

        var normalizedPrefixResult = NormalizePrefix(prefix);
        if (normalizedPrefixResult.IsFailure)
            return Result.Failure<StorageKey, Error>(normalizedPrefixResult.Error);

        var normalizedKeyResult = NormalizeSegment(key);
        if (normalizedKeyResult.IsFailure)
            return Result.Failure<StorageKey, Error>(normalizedKeyResult.Error);

        return Result.Success<StorageKey, Error>(
            new StorageKey(location.Trim(), normalizedPrefixResult.Value, normalizedKeyResult.Value));
    }

    private static Result<string, Error> NormalizePrefix(string? prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            return Result.Success<string, Error>(string.Empty);

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

        return Result.Success<string, Error>(string.Join("/", normalizedParts));
    }

    private static Result<string, Error> NormalizeSegment(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<string, Error>(Error.Failure("empty.value", "cannot be empty"));

        string trimmed = value!.Trim();

        if (trimmed.Contains('/', StringComparison.Ordinal) || trimmed.Contains('\\', StringComparison.Ordinal))
            return Result.Failure<string, Error>(Error.Failure("invalid.key", "invalid.key"));

        return Result.Success<string, Error>(trimmed);
    }
}