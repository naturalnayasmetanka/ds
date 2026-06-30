namespace FS.Core.Options;

public record S3Options
{
    public string Endpoint { get; init; } = string.Empty;

    public string AccessKey { get; init; } = string.Empty;

    public string SecretKey { get; init; } = string.Empty;

    public bool WithSsl { get; init; }

    public int DownloadUrlExpirationHours { get; init; } = 24;

    public IReadOnlyList<string> RequiredBuckets { get; init; } = [];
    public double UploadUrlExpirationHours { get; init; } = 1;

    public int MaxCuncurrentRequests { get; init; } = 20;
    public long ChunkSize { get; init; } = 100 * 1024 * 1024;
    public int MaxChunks { get; init; } = 1000;
}
