using CSharpFunctionalExtensions;
using FS.Core.Enums;
using FS.Core.Exceptions;
using FS.Core.ValueObjects;

namespace FS.Core.Entities;

public abstract class MediaAsset
{
    protected MediaAsset()
    {

    }

    protected MediaAsset(
        Guid id,
        MediaData mediaData,
        MediaStatus mediaStatus,
        AssetType assetType,
        StorageKey key)
    {
        Id = id;
        MediaData = mediaData;
        AssetType = assetType;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        MediaStatus = mediaStatus;
        Key = key;
    }

    public Guid Id { get; protected set; }
    public MediaData MediaData { get; protected set; }
    public AssetType AssetType { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
    public StorageKey Key { get; protected set; }
    public MediaStatus MediaStatus { get; protected set; }
    public ActualMediaData? ActualData { get; protected set; }


    public static Result<MediaAsset, Error> CreateForUpload(MediaData mediaData, AssetType assetType)
    {
        switch (assetType)
        {
            case AssetType.IMAGE:
                var result = ImageAsset.CreateForUpload(Guid.CreateVersion7(), mediaData);
                return result.IsFailure
                    ? Result.Failure<MediaAsset, Error>(Error.Failure("create.error", "image asset create error"))
                    : Result.Success<MediaAsset, Error>(result.Value);

            default:
                throw new ArgumentOutOfRangeException(nameof(assetType), assetType, null);
        }
    }

    public UnitResult<Error> MarkUploaded()
    {
        if (MediaStatus != MediaStatus.UPLOADING)
            return Error.Failure("invalid.transition", $"Cannot mark as uploaded from {MediaStatus}");

        MediaStatus = MediaStatus.UPLOADED;
        UpdatedAt = DateTime.UtcNow;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> MarkReady(StorageKey key)
    {
        if (MediaStatus != MediaStatus.UPLOADED)
            return Error.Failure("invalid.transition", $"Cannot mark as ready from {MediaStatus}");

        Key = key;
        MediaStatus = MediaStatus.READY;
        UpdatedAt = DateTime.UtcNow;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> MarkFailed()
    {
        if (MediaStatus is not (MediaStatus.UPLOADING or MediaStatus.UPLOADED))
            return Error.Failure("invalid.transition", $"Cannot mark as failed from {MediaStatus}");

        MediaStatus = MediaStatus.FAILED;
        UpdatedAt = DateTime.UtcNow;
        return UnitResult.Success<Error>();
    }

    /// <summary>
    /// Завершает простую (single PUT) загрузку: проверяет, что asset находится
    /// в ожидаемом состоянии, сверяет фактические метаданные объекта в storage
    /// с заявленными при инициации/политикой типа asset'а, и переводит asset в READY.
    /// Не изменяет состояние при конфликте статуса (повторный/чужой/неуместный вызов),
    /// но помечает asset как FAILED, если объект загружен, но не прошёл проверку соответствия.
    /// </summary>
    public UnitResult<Error> CompleteUpload(ActualMediaData actual)
    {
        if (MediaStatus != MediaStatus.UPLOADING)
            return Error.Conflict("invalid.transition", $"Cannot complete upload from {MediaStatus}");

        var policyResult = ValidateActual(actual);
        if (policyResult.IsFailure)
        {
            MediaStatus = MediaStatus.FAILED;
            UpdatedAt = DateTime.UtcNow;
            return policyResult.Error;
        }

        ActualData = actual;
        MediaStatus = MediaStatus.READY;
        UpdatedAt = DateTime.UtcNow;
        return UnitResult.Success<Error>();
    }

    protected abstract UnitResult<Error> ValidateActual(ActualMediaData actual);

    public UnitResult<Error> Delete()
    {
        if (MediaStatus == MediaStatus.DELETED)
            return Error.Failure("invalid.transition", "Already deleted");

        MediaStatus = MediaStatus.DELETED;
        UpdatedAt = DateTime.UtcNow;
        return UnitResult.Success<Error>();
    }

}