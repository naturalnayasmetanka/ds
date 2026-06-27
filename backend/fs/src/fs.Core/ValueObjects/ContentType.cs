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
            return Result.Failure<ContentType, Error>(Error.Failure("empty.contenttype", "connot be empty"));

        MediaType category = contentType switch
        {
            var ct when ct.Contains("video", StringComparison.InvariantCultureIgnoreCase) => MediaType.VIDEO,
            var ct when ct.Contains("image", StringComparison.InvariantCultureIgnoreCase) => MediaType.IMAGE,
            var ct when ct.Contains("audio", StringComparison.InvariantCultureIgnoreCase) => MediaType.AUDIO,
            var ct when ct.Contains("document", StringComparison.InvariantCultureIgnoreCase) => MediaType.DOCUMENT,

            _ => MediaType.UNKNOWN

        };

        return Result.Success<ContentType, Error>(new ContentType(contentType, category));
    }
}