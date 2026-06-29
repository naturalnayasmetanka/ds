using Amazon.S3;
using Amazon.S3.Model;
using CSharpFunctionalExtensions;
using FS.Contracts;
using FS.Core.Abstractions;
using FS.Core.Exceptions;
using FS.Core.Options;
using FS.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FS.Infrastructure.S3;

public class S3Provider : IS3Provider, IDisposable
{
    private readonly ILogger<S3Provider> _logger;
    private readonly IAmazonS3 _amazonS3;
    private IOptions<S3Options> _s3Options;

    private readonly SemaphoreSlim _requestsSemaphore;

    public S3Provider(IAmazonS3 s3client, ILogger<S3Provider> logger, IOptions<S3Options> options)
    {
        _amazonS3 = s3client;
        _logger = logger;
        _s3Options = options;
        _requestsSemaphore = new SemaphoreSlim(1, _s3Options.Value.MaxCuncurrentRequests);
    }

    public async Task UploadFileAsync(Stream stream, string bucketName, string key, string contentType, CancellationToken cancellationToken)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = stream,
            ContentType = contentType,
        };


        await _amazonS3.PutObjectAsync(request, cancellationToken);
    }

    public async Task<Result<string, Error>> StartMultipartUploadAsync(
        StorageKey storageKey, MediaData mediaData, CancellationToken cancellationToken)
    {
        try
        {
            var request = new InitiateMultipartUploadRequest
            {
                BucketName = storageKey.Location,
                Key = storageKey.Value,
                ContentType = mediaData.ContentType.Value,
            };

            var result = await _amazonS3.InitiateMultipartUploadAsync(request, cancellationToken);

            return Result.Success<string, Error>(result.UploadId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting multipart uploading");

            return Result.Failure<string, Error>(Error.Failure("multipart.upload.error", ex.Message));
        }
    }

    public async Task<Result<IReadOnlyList<ChunkUploadUrl>, Error>> GenerateAllChunksUploadAsync(
        StorageKey storageKey, string uploadId, int totalChunks, CancellationToken cancellationToken)
    {
        var tasks = Enumerable.Range(1, totalChunks)
            .Select(async partNumber =>
            {
                await _requestsSemaphore.WaitAsync(cancellationToken);

                try
                {
                    var request = new GetPreSignedUrlRequest
                    {
                        BucketName = storageKey.Location,
                        Key = storageKey.Value,
                        Verb = HttpVerb.PUT,
                        UploadId = uploadId,
                        PartNumber = partNumber,
                        Expires = DateTime.UtcNow.AddHours(_s3Options.Value.UploadUrlExpirationHours),
                        Protocol = _s3Options.Value.WithSsl ? Protocol.HTTPS : Protocol.HTTP
                    };

                    var url = await _amazonS3.GetPreSignedURLAsync(request);

                    return new ChunkUploadUrl(partNumber, url);
                }
                finally
                {
                    _requestsSemaphore.Release();
                }
            });

        var results = await Task.WhenAll(tasks);

        return Result.Success<IReadOnlyList<ChunkUploadUrl>, Error>(results);
    }

    public async Task<string> GenerateDownloadUrl(StorageKey storageKey)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = storageKey.Location,
            Key = storageKey.Value,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddHours(_s3Options.Value.DownloadUrlExpirationHours),
            Protocol = _s3Options.Value.WithSsl ? Protocol.HTTPS : Protocol.HTTP
        };

        var response = await _amazonS3.GetPreSignedURLAsync(request);

        return response;
    }

    public async Task<Result<string?, Error>> CompleteMultipartUploadAsync(
        StorageKey storageKey, string uploadId, List<PartEtagDto> partETags, CancellationToken cancellationToken)
    {
        try
        {
            var request = new Amazon.S3.Model.CompleteMultipartUploadRequest
            {
                BucketName = storageKey.Location,
                Key = storageKey.Value,
                UploadId = uploadId,
                PartETags = partETags.Select(x => new PartETag { ETag = x.ETag, PartNumber = x.PartNumber }).ToList()
            };

            var result = await _amazonS3.CompleteMultipartUploadAsync(request, cancellationToken);


            return Result.Success<string?, Error>(result.Key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing multipart uploading");
            return Result.Failure<string?, Error>(Error.Failure("multipart.upload.error", ex.Message));
        }
    }

    public void Dispose()
    {
        _requestsSemaphore.Dispose();
    }
}
