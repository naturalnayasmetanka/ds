using CSharpFunctionalExtensions;
using FS.Core.Enums;
using FS.Core.Exceptions;

namespace FS.Core.ValueObjects;

public sealed record ContentType
{
    public string Value { get; }
    public MediaType Category { get; }

    private ContentType(string value, MediaType category)
    {
        Value = value;
        Category = category;
    }

    public static Result<ContentType, Error> Create(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return Result.Failure<ContentType, Error>(Error.Failure("empty.contenttype", "cannot be empty"));

        var normalized = contentType.Trim().ToLowerInvariant();

        var category = _mimeToCategory.TryGetValue(normalized, out var mediaType)
            ? mediaType
            : MediaType.UNKNOWN;

        return Result.Success<ContentType, Error>(new ContentType(normalized, category));
    }

    private static readonly Dictionary<string, MediaType> _mimeToCategory = new()
    {
        ["video/mp4"] = MediaType.VIDEO,
        ["video/x-matroska"] = MediaType.VIDEO,
        ["video/x-msvideo"] = MediaType.VIDEO,
        ["video/quicktime"] = MediaType.VIDEO,

        ["image/jpeg"] = MediaType.IMAGE,
        ["image/png"] = MediaType.IMAGE,
        ["image/gif"] = MediaType.IMAGE,
        ["image/webp"] = MediaType.IMAGE,
        ["image/svg+xml"] = MediaType.IMAGE,

        ["audio/mpeg"] = MediaType.AUDIO,
        ["audio/ogg"] = MediaType.AUDIO,
        ["audio/wav"] = MediaType.AUDIO,

        ["application/pdf"] = MediaType.DOCUMENT,
        ["application/msword"] = MediaType.DOCUMENT,
        ["application/vnd.openxmlformats-officedocument.wordprocessingml.document"] = MediaType.DOCUMENT,
    };
}