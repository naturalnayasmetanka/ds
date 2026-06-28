using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using FluentAssertions;
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

    [Fact]
    public async Task UploadFile_ThenDownload_ContentMatches()
    {
        var ct = CancellationToken.None;
        var content = "hello from integration test"u8.ToArray();

        // 1. Загружаем файл
        using var uploadStream = new MemoryStream(content);
        await _s3Provider.UploadFileAsync(uploadStream, BucketName, Key, ContentType, ct);

        // 2. Получаем download URL
        var downloadUrl = await _s3Provider.GenerateDownloadUrl(BucketName, Key);
        downloadUrl.Should().NotBeNullOrEmpty();

        // 3. Скачиваем и проверяем содержимое
        using var httpClient = new HttpClient();
        var downloaded = await httpClient.GetByteArrayAsync(downloadUrl);
        downloaded.Should().BeEquivalentTo(content);

        // 4. Удаляем объект
        await _amazonS3.DeleteObjectAsync(BucketName, Key, ct);

        // 5. Проверяем что удалён — GetObjectMetadataAsync должен бросить исключение
        var act = async () => await _amazonS3.GetObjectMetadataAsync(BucketName, Key, ct);
        await act.Should().ThrowAsync<AmazonS3Exception>()
            .Where(e => e.StatusCode == System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GeneratePresignedUploadUrl_ThenUploadViaHttp_ContentMatches()
    {
        var key = $"test/{Guid.NewGuid()}.txt";
        var content = "uploaded via presigned url"u8.ToArray();

        // 1. Генерируем presigned upload URL
        var uploadUrl = await _s3Provider.GenerateUploadUrlAsync(BucketName, key);
        uploadUrl.Should().NotBeNullOrEmpty();

        // 2. Загружаем файл напрямую через HTTP PUT (как это делает фронтенд)
        using var httpClient = new HttpClient();
        var putResponse = await httpClient.PutAsync(
            uploadUrl,
            new ByteArrayContent(content));

        putResponse.IsSuccessStatusCode.Should().BeTrue(
            $"presigned upload failed with {putResponse.StatusCode}");

        // 3. Скачиваем и сверяем содержимое
        var downloadUrl = await _s3Provider.GenerateDownloadUrl(BucketName, key);
        var downloaded = await httpClient.GetByteArrayAsync(downloadUrl);
        downloaded.Should().BeEquivalentTo(content);
    }
}