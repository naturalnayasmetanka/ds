namespace FS.Contracts;

public record StartMultipartUploadResponse(
    Guid MediaAssetId, string UploadId, IReadOnlyList<ChunkUploadUrl> ChunkUploadUrls, long chunkSize);