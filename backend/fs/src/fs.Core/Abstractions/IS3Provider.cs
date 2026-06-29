using CSharpFunctionalExtensions;
using FS.Contracts;
using FS.Core.Exceptions;
using FS.Core.ValueObjects;

namespace FS.Core.Abstractions;

public interface IS3Provider
{
    Task<Result<string, Error>> StartMultipartUploadAsync(
        StorageKey storageKey, MediaData mediaData, CancellationToken cancellationToken);

    Task<Result<IReadOnlyList<ChunkUploadUrl>, Error>> GenerateAllChunksUploadAsync(
        StorageKey storageKey, string uploadId, int totalChunks, CancellationToken cancellationToken);

    Task<Result<string?, Error>> CompleteMultipartUploadAsync(
        StorageKey storageKey, string uploadId, List<PartEtagDto> partETags, CancellationToken cancellationToken);

    Task<string> GenerateDownloadUrl(StorageKey storageKey);

    Task UploadFileAsync(Stream stream, string bucketName, string key, string contentType, CancellationToken cancellationToken);
}
