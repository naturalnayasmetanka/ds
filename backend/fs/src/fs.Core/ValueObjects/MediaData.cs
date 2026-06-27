using CSharpFunctionalExtensions;
using FS.Core.Exceptions;

namespace FS.Core.ValueObjects;

public sealed record MediaData
{
    public FileName FileName { get; }
    public ContentType ContentType { get; }
    public long Size { get; }
    public int ExpectedChunksCount { get; }

    public MediaData(FileName fileName, ContentType contentType, long size, int expectedChunksCount)
    {
        FileName = fileName;
        ContentType = contentType;
        Size = size;
        ExpectedChunksCount = expectedChunksCount;
    }

    public static Result<MediaData, Error> Create(
        FileName fileName, ContentType contentType, long size, int expectedChunksCount)
    {
        if (size <= 0)
            return Result.Failure<MediaData, Error>(Error.Failure("empty.size", "cannot be less or eq 0"));

        if (expectedChunksCount <= 0)
            return Result.Failure<MediaData, Error>(Error.Failure("empty.expectedChunksCount", "cannot be less or eq 0"));

        return Result.Success<MediaData, Error>(new MediaData(fileName, contentType, size, expectedChunksCount));
    }
}