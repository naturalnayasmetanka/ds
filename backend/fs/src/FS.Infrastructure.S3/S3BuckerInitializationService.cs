using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using FS.Core.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FS.Infrastructure.S3;

public class S3BuckerInitializationService : BackgroundService
{
    private readonly IAmazonS3 _s3;
    private readonly ILogger<S3BuckerInitializationService> _logger;
    private readonly IOptions<S3Options> _options;

    public S3BuckerInitializationService(
        IAmazonS3 s3, ILogger<S3BuckerInitializationService> logger, IOptions<S3Options> options)
    {
        _s3 = s3;
        _logger = logger;
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("S3 bucket initialization service is starting.");

            if (_options.Value.RequiredBuckets.Count == 0)
            {
                _logger.LogInformation("No required buckets specified. Skipping bucket initialization.");
                throw new ArgumentException("No required buckets specified. Skipping bucket initialization.");
            }

            _logger.LogInformation("Checking required S3 buckets... {Buckets}", string.Join(",", _options.Value.RequiredBuckets));

            Task[] tasks = _options.Value.RequiredBuckets
                 .Select(bucketName => InitializeBucketAsync(bucketName, stoppingToken)).ToArray();

            await Task.WhenAll(tasks);
        }
        catch (OperationCanceledException ex)
        {

            _logger.LogCritical(ex, "S3 bucket initialization service failed to start.");
        }
        catch (Exception)
        {

            _logger.LogCritical("S3 bucket initialization service failed to start.");
        }
    }

    private async Task InitializeBucketAsync(string bucketName, CancellationToken cancellationToken)
    {
        try
        {
            bool bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3, bucketName);

            if (bucketExists)
            {
                _logger.LogInformation("Bucket {Bucket} already exists", bucketName);
                return;
            }

            _logger.LogInformation("Creating bucket '{BucketName}'", bucketName);

            var putBucketRequest = new PutBucketRequest
            {
                BucketName = bucketName,
            };

            await _s3.PutBucketAsync(putBucketRequest, cancellationToken);

            string policy = $$"""
                                    {
                                        "Statement": [
                                            {
                                                "Version": "2012-10-17",
                                                "Effect": "Allow",
                                                "Principal": {
                                                    "AWS": ["*"]
                                                },
                                                "Action": ["s3:GetObject"],
                                                "Resource": ["arn:aws:s3:::{{bucketName}}/*"]
                                            }
                                        ]
                                    }
                                    """;

            var putPolicyRequest = new PutBucketPolicyRequest
            {
                BucketName = bucketName,
                Policy = policy
            };

            await _s3.PutBucketPolicyAsync(putPolicyRequest, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to initialize bucket {BucketName}", bucketName);
        }
    }
}
