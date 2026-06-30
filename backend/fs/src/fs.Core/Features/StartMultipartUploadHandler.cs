using CSharpFunctionalExtensions;
using FS.Contracts;
using FS.Core.Abstractions;
using FS.Core.Abstractions.Common;
using FS.Core.Entities;
using FS.Core.Exceptions;
using FS.Core.Extentions;
using FS.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace FS.Core.Features;

public record StartMultipartUploadCommand(StartMultipartUploadRequest request) : ICommand;

public sealed class StartMultipartUploadHandler : ICommandHandler<StartMultipartUploadResponse, StartMultipartUploadCommand>
{
    private readonly ILogger<StartMultipartUploadHandler> _logger;
    private readonly IS3Provider _s3Provider;
    private readonly IChunkSizeCalculator _chunkSizeCalculator;
    private readonly IMediaAssetRepository _mediaAssetRepository;

    public StartMultipartUploadHandler(
        ILogger<StartMultipartUploadHandler> logger,
        IS3Provider s3Provider,
        IChunkSizeCalculator chunkSizeCalculator,
        IMediaAssetRepository mediaAssetRepository)
    {
        _logger = logger;
        _s3Provider = s3Provider;
        _chunkSizeCalculator = chunkSizeCalculator;
        _mediaAssetRepository = mediaAssetRepository;
    }

    public async Task<Result<StartMultipartUploadResponse, Errors>> Handle(
        StartMultipartUploadCommand command, CancellationToken cancellationToken = default)
    {
        var fileNameResult = FileName.Create(command.request.FileName);
        if (fileNameResult.IsFailure)
            Result.Failure<Guid, Errors>(Error.Validation("filename.invalid", "Name is incorrect", nameof(FileName)));

        var contentTypeResult = ContentType.Create(command.request.ContentType);
        if (contentTypeResult.IsFailure)
            Result.Failure<Guid, Errors>(Error.Validation("contenttype.invalid", "ContentType is incorrect", nameof(ContentType)));

        var chunkSizeResult = _chunkSizeCalculator.Calculate(command.request.Size);

        var mediaData = MediaData.Create(
            fileNameResult.Value, contentTypeResult.Value, command.request.Size, chunkSizeResult.Value.TotalChunks);

        var mediaAssetResult = MediaAsset.CreateForUpload(
            mediaData.Value, command.request.AssetType.ToAssetType());

        await _mediaAssetRepository.AddAsync(mediaAssetResult.Value, cancellationToken);

        var startUploadResult = await _s3Provider.StartMultipartUploadAsync(
            mediaAssetResult.Value.Key,
            mediaAssetResult.Value.MediaData,
            cancellationToken);

        var chunkUploadUrlsResult = await _s3Provider.GenerateAllChunksUploadAsync(
            mediaAssetResult.Value.Key,
            startUploadResult.Value,
            chunkSizeResult.Value.TotalChunks,
            cancellationToken);

        _logger.LogInformation("Media Asset is starting to upload, {Id}", mediaAssetResult.Value.Id);

        return Result.Success<StartMultipartUploadResponse, Errors>(new StartMultipartUploadResponse(
                mediaAssetResult.Value.Id,
                startUploadResult.Value,
                chunkUploadUrlsResult.Value,
                chunkSizeResult.Value.ChunkSize));
    }
}