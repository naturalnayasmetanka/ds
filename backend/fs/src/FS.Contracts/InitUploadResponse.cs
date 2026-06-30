namespace FS.Contracts;

public record InitUploadResponse(
    Guid MediaAssetId,
    string UploadUrl,
    string HttpMethod,
    DateTime ExpiresAt,
    IReadOnlyDictionary<string, string> RequiredHeaders);
