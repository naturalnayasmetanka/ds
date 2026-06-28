using Amazon.S3.Model;
using CSharpFunctionalExtensions;
using FS.Core.Exceptions;

namespace FS.Core.Abstractions;

public interface IS3Provider
{
    Task<Result<string, Error>> StartMultipartUploadAsync(
        string bucketName, string key, string contentType, CancellationToken cancellationToken);

    Task<Result<IReadOnlyList<string>, Error>> GenerateAllChunksUploadAsync(
        string bucketName, string key, string uploadId, int totalChunks, CancellationToken cancellationToken);

    Task<Result<CompleteMultipartUploadResponse?, Error>> CompleteMultipartUploadAsync(
        string bucketName, string key, string uploadId, List<PartETag> partETags, CancellationToken cancellationToken);

    Task<string> GenerateDownloadUrl(string bucketName, string key);

    Task<string> GenerateUploadUrlAsync(string bucketName, string key);

    Task UploadFileAsync(Stream stream, string bucketName, string key, string contentType, CancellationToken cancellationToken);
}
