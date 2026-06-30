namespace FS.Contracts;

public record CompleteUploadResponse(
    Guid MediaAssetId,
    string Status,
    long Size,
    string ContentType,
    string ETag,
    string StorageKey);
