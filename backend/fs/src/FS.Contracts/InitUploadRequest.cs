namespace FS.Contracts;

public record InitUploadRequest(
    string FileName,
    string AssetType,
    string ContentType,
    long Size,
    string Context,
    Guid ContextId);
