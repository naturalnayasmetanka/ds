using CSharpFunctionalExtensions;
using FS.Core.Exceptions;

namespace FS.Core.ValueObjects;

/// <summary>
/// Фактические метаданные объекта, полученные напрямую из storage provider
/// после завершения загрузки (HEAD на объект). В отличие от <see cref="MediaData"/>,
/// который описывает заявленные клиентом при инициации параметры.
/// </summary>
public sealed record ActualMediaData
{
    public long Size { get; }
    public string ContentType { get; }
    public string ETag { get; }

    protected ActualMediaData()
    {
    }

    private ActualMediaData(long size, string contentType, string eTag)
    {
        Size = size;
        ContentType = contentType;
        ETag = eTag;
    }

    public static Result<ActualMediaData, Error> Create(long size, string contentType, string eTag)
    {
        if (size <= 0)
            return Result.Failure<ActualMediaData, Error>(Error.Failure("empty.size", "cannot be less or eq 0"));

        if (string.IsNullOrWhiteSpace(contentType))
            return Result.Failure<ActualMediaData, Error>(Error.Failure("empty.contenttype", "cannot be empty"));

        if (string.IsNullOrWhiteSpace(eTag))
            return Result.Failure<ActualMediaData, Error>(Error.Failure("empty.etag", "cannot be empty"));

        return Result.Success<ActualMediaData, Error>(new ActualMediaData(size, contentType.Trim().ToLowerInvariant(), eTag.Trim()));
    }
}
