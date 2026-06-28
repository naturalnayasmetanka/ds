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
        MediaOwner mediaOwner)
    {
        Id = id;
        MediaData = mediaData;
        AssetType = assetType;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        MediaOwner = mediaOwner;
        MediaStatus = mediaStatus;
    }

    public Guid Id { get; protected set; }
    public MediaData MediaData { get; protected set; }
    public AssetType AssetType { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
    public StorageKey Key { get; protected set; }
    public MediaOwner MediaOwner { get; protected set; } = null!;
    public MediaStatus MediaStatus { get; protected set; }


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

    public UnitResult<Error> Delete()
    {
        if (MediaStatus == MediaStatus.DELETED)
            return Error.Failure("invalid.transition", "Already deleted");

        MediaStatus = MediaStatus.DELETED;
        UpdatedAt = DateTime.UtcNow;
        return UnitResult.Success<Error>();
    }

}