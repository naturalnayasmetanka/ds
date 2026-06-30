using CSharpFunctionalExtensions;
using FS.Contracts;
using FS.Core.Abstractions;
using FS.Core.Abstractions.Common;
using FS.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace FS.Core.Features;

public record CompleteMultipartUploadCommand(CompleteMultipartUploadRequest request) : ICommand;

public sealed class CompleteMultipartUploadHandler : ICommandHandler<CompleteMultipartUploadResponse, CompleteMultipartUploadCommand>
{
    private readonly ILogger<CompleteMultipartUploadHandler> _logger;
    private readonly IS3Provider _s3Provider;
    private readonly IMediaAssetRepository _mediaAssetRepository;

    public CompleteMultipartUploadHandler(
        ILogger<CompleteMultipartUploadHandler> logger,
        IS3Provider s3Provider,
        IMediaAssetRepository mediaAssetRepository)
    {
        _logger = logger;
        _s3Provider = s3Provider;
        _mediaAssetRepository = mediaAssetRepository;
    }

    public async Task<Result<CompleteMultipartUploadResponse, Errors>> Handle(
        CompleteMultipartUploadCommand command, CancellationToken cancellationToken = default)
    {
        var mediaAsset = await _mediaAssetRepository
            .GetBy(m => m.Id == command.request.MediaAssetId, cancellationToken);

        if (mediaAsset.IsFailure)
            return Result.Failure<CompleteMultipartUploadResponse, Errors>(mediaAsset.Error);

        if (mediaAsset.Value.MediaData.ExpectedChunksCount != command.request.PartEtags.Count)
            return Result.Failure<CompleteMultipartUploadResponse, Errors>(Error.Failure("count.error", "chunks error"));

        var completeResult = await _s3Provider.CompleteMultipartUploadAsync(
            mediaAsset.Value.Key,
            command.request.UploadId,
            command.request.PartEtags.ToList(),
            cancellationToken);

        if (completeResult.IsFailure)
        {
            mediaAsset.Value.MarkFailed();

            await _mediaAssetRepository.SaveAsync(cancellationToken);

            return Result.Failure<CompleteMultipartUploadResponse, Errors>(completeResult.Error);
        }


        mediaAsset.Value.MarkUploaded();

        await _mediaAssetRepository.SaveAsync(cancellationToken);

        _logger.LogInformation("File uploaded successfully. MediaAssetId: {MediaAssetId}", mediaAsset.Value.Id);

        return Result.Success<CompleteMultipartUploadResponse, Errors>(new CompleteMultipartUploadResponse(mediaAsset.Value.Id));
    }
}
