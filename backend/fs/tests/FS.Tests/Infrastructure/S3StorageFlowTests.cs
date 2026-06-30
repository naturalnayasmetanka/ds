using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using FluentAssertions;
using FS.Core.Options;
using FS.Infrastructure.S3;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Testcontainers.Minio;

namespace FS.Tests.Infrastructure;

public class S3StorageFlowTests : IAsyncLifetime
{
    private readonly MinioContainer _minioContainer = new MinioBuilder()
        .WithImage("minio/minio:RELEASE.2024-01-16T16-07-38Z")
        .Build();

    private S3Provider _s3Provider = null!;
    private IAmazonS3 _amazonS3 = null!;

    private const string BucketName = "test-bucket";
    private const string Key = "test/file.txt";
    private const string ContentType = "text/plain";

    public async Task InitializeAsync()
    {
        await _minioContainer.StartAsync();

        var options = new S3Options
        {
            Endpoint = _minioContainer.GetConnectionString(),
            AccessKey = _minioContainer.GetAccessKey(),
            SecretKey = _minioContainer.GetSecretKey(),
            WithSsl = false,
            DownloadUrlExpirationHours = 1,
            UploadUrlExpirationHours = 1,
            MaxCuncurrentRequests = 5,
            RequiredBuckets = []
        };

        var config = new AmazonS3Config
        {
            ServiceURL = options.Endpoint,
            ForcePathStyle = true,  // <- критично для MinIO
            UseHttp = true,
        };

        _amazonS3 = new AmazonS3Client(
            options.AccessKey,
            options.SecretKey,
            config);

        _s3Provider = new S3Provider(
            _amazonS3,
            NullLogger<S3Provider>.Instance,
            Options.Create(options));

        await _amazonS3.PutBucketAsync(BucketName);
    }

    public async Task DisposeAsync()
    {
        _s3Provider.Dispose();
        _amazonS3.Dispose();
        await _minioContainer.DisposeAsync();
    }
}