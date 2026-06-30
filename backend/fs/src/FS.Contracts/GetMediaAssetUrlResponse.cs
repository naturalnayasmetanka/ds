namespace FS.Contracts;

public record GetMediaAssetUrlResponse(
    Guid MediaAssetId,
    string Url,
    DateTime ExpiresAt);
