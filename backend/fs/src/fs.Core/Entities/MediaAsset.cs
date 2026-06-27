using FS.Core.Enums;
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

}