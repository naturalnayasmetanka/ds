using FS.Core.Enums;

namespace FS.Core.Extentions;

public static class AssetTypeExtentions
{
    public static AssetType ToAssetType(this string value)
    {
        return value.ToLowerInvariant() switch
        {
            "video" => AssetType.VIDEO,
            "preview" => AssetType.PREVIEW,
            "image" => AssetType.IMAGE,
            "avatar" => AssetType.AVATAR,
            _ => throw new ArgumentException($"Invalid asset type {value}")
        };
    }
}