namespace FS.Contracts;

public record CompleteMultipartUploadRequest(Guid MediaAssetId, string UploadId, IReadOnlyList<PartEtagDto> PartEtags);
