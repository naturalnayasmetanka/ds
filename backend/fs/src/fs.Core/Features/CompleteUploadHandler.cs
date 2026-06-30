using CSharpFunctionalExtensions;
using FS.Contracts;
using FS.Core.Abstractions;
using FS.Core.Abstractions.Common;
using FS.Core.Enums;
using FS.Core.Exceptions;
using FS.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace FS.Core.Features;

public record CompleteUploadCommand(CompleteUploadRequest Request) : ICommand;

/// <summary>
/// Шаг 3: подтверждение загрузки. Не доверяет клиенту — реальное состояние
/// объекта в storage всегда перепроверяется через HEAD-запрос к provider'у
/// перед переводом asset'а в READY.
/// </summary>
public sealed class CompleteUploadHandler : ICommandHandler<CompleteUploadResponse, CompleteUploadCommand>
{
    private readonly ILogger<CompleteUploadHandler> _logger;
    private readonly IS3Provider _s3Provider;
    private readonly IMediaAssetRepository _mediaAssetRepository;

    public CompleteUploadHandler(
        ILogger<CompleteUploadHandler> logger,
        IS3Provider s3Provider,
        IMediaAssetRepository mediaAssetRepository)
    {
        _logger = logger;
        _s3Provider = s3Provider;
        _mediaAssetRepository = mediaAssetRepository;
    }

    public async Task<Result<CompleteUploadResponse, Errors>> Handle(
        CompleteUploadCommand command, CancellationToken cancellationToken = default)
    {
        var assetResult = await _mediaAssetRepository.GetBy(
            m => m.Id == command.Request.MediaAssetId && m.MediaStatus != MediaStatus.DELETED,
            cancellationToken);

        if (assetResult.IsFailure)
            return Result.Failure<CompleteUploadResponse, Errors>(assetResult.Error);

        var asset = assetResult.Value;

        // Повторный/чужой вызов completion на уже готовый asset — явный конфликт, а не успех.
        if (asset.MediaStatus == MediaStatus.READY)
            return Result.Failure<CompleteUploadResponse, Errors>(
                Error.Conflict("upload.already.completed", "This upload has already been completed"));

        if (asset.MediaStatus != MediaStatus.UPLOADING)
            return Result.Failure<CompleteUploadResponse, Errors>(
                Error.Conflict("upload.invalid.state", $"Cannot complete upload from status {asset.MediaStatus}"));

        // Не доверяем клиенту: реальное наличие и метаданные объекта читаем из storage.
        var metadataResult = await _s3Provider.GetObjectMetadataAsync(asset.Key, cancellationToken);
        if (metadataResult.IsFailure)
        {
            _logger.LogWarning(
                "Completion attempted for media asset {Id} but object is not present in storage", asset.Id);

            return Result.Failure<CompleteUploadResponse, Errors>(metadataResult.Error);
        }

        var actualDataResult = ActualMediaData.Create(
            metadataResult.Value.Size, metadataResult.Value.ContentType, metadataResult.Value.ETag);
        if (actualDataResult.IsFailure)
            return Result.Failure<CompleteUploadResponse, Errors>(actualDataResult.Error);

        var completeResult = asset.CompleteUpload(actualDataResult.Value);

        await _mediaAssetRepository.SaveAsync(cancellationToken);

        if (completeResult.IsFailure)
        {
            _logger.LogWarning(
                "Media asset {Id} failed completion policy check: {Error}", asset.Id, completeResult.Error.Code);

            return Result.Failure<CompleteUploadResponse, Errors>(completeResult.Error);
        }

        _logger.LogInformation("Media asset {Id} completed and marked READY", asset.Id);

        return Result.Success<CompleteUploadResponse, Errors>(new CompleteUploadResponse(
            asset.Id,
            asset.MediaStatus.ToString(),
            asset.ActualData!.Size,
            asset.ActualData.ContentType,
            asset.ActualData.ETag,
            asset.Key.FullPath));
    }
}
