using CSharpFunctionalExtensions;
using FS.Contracts;
using FS.Core.Abstractions;
using FS.Core.Abstractions.Common;
using FS.Core.Entities;
using FS.Core.Enums;
using FS.Core.Exceptions;
using FS.Core.Extentions;
using FS.Core.Options;
using FS.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FS.Core.Features;

public record InitUploadCommand(InitUploadRequest Request) : ICommand;

/// <summary>
/// Шаг 1: регистрация asset'а в состоянии ожидания загрузки и выдача
/// presigned PUT URL для прямой загрузки в storage (без multipart).
/// </summary>
public sealed class InitUploadHandler : ICommandHandler<InitUploadResponse, InitUploadCommand>
{
    private readonly ILogger<InitUploadHandler> _logger;
    private readonly IS3Provider _s3Provider;
    private readonly IMediaAssetRepository _mediaAssetRepository;
    private readonly IOptions<S3Options> _s3Options;

    public InitUploadHandler(
        ILogger<InitUploadHandler> logger,
        IS3Provider s3Provider,
        IMediaAssetRepository mediaAssetRepository,
        IOptions<S3Options> s3Options)
    {
        _logger = logger;
        _s3Provider = s3Provider;
        _mediaAssetRepository = mediaAssetRepository;
        _s3Options = s3Options;
    }

    public async Task<Result<InitUploadResponse, Errors>> Handle(
        InitUploadCommand command, CancellationToken cancellationToken = default)
    {
        var request = command.Request;

        var fileNameResult = FileName.Create(request.FileName);
        if (fileNameResult.IsFailure)
            return Result.Failure<InitUploadResponse, Errors>(
                Error.Validation("filename.invalid", "Name is incorrect", nameof(FileName)));

        var contentTypeResult = ContentType.Create(request.ContentType);
        if (contentTypeResult.IsFailure)
            return Result.Failure<InitUploadResponse, Errors>(
                Error.Validation("contenttype.invalid", "ContentType is incorrect", nameof(ContentType)));

        var mediaDataResult = MediaData.Create(fileNameResult.Value, contentTypeResult.Value, request.Size, 1);
        if (mediaDataResult.IsFailure)
            return Result.Failure<InitUploadResponse, Errors>(mediaDataResult.Error);

        AssetType assetType;
        try
        {
            assetType = request.AssetType.ToAssetType();
        }
        catch (ArgumentException)
        {
            return Result.Failure<InitUploadResponse, Errors>(
                Error.Validation("assettype.invalid", $"Unknown asset type {request.AssetType}", nameof(request.AssetType)));
        }

        // FS-1: применение политик (формат/размер/тип) происходит внутри MediaAsset.CreateForUpload
        // (см. ImageAsset.Validate и аналогичные правила для других типов asset'ов).
        Result<MediaAsset, Error> mediaAssetResult;
        try
        {
            mediaAssetResult = MediaAsset.CreateForUpload(mediaDataResult.Value, assetType);
        }
        catch (ArgumentOutOfRangeException)
        {
            return Result.Failure<InitUploadResponse, Errors>(
                Error.Validation("assettype.unsupported", $"Asset type {assetType} is not supported yet", nameof(request.AssetType)));
        }

        if (mediaAssetResult.IsFailure)
            return Result.Failure<InitUploadResponse, Errors>(mediaAssetResult.Error);

        var asset = mediaAssetResult.Value;

        var addResult = await _mediaAssetRepository.AddAsync(asset, cancellationToken);
        if (addResult.IsFailure)
            return Result.Failure<InitUploadResponse, Errors>(addResult.Error);

        var uploadUrl = await _s3Provider.GenerateSimpleUploadUrlAsync(
            asset.Key, asset.MediaData.ContentType.Value, cancellationToken);

        var expiresAt = DateTime.UtcNow.AddHours(_s3Options.Value.UploadUrlExpirationHours);

        var requiredHeaders = new Dictionary<string, string>
        {
            ["Content-Type"] = asset.MediaData.ContentType.Value,
        };

        _logger.LogInformation("Media asset {Id} registered for upload", asset.Id);

        return Result.Success<InitUploadResponse, Errors>(new InitUploadResponse(
            asset.Id,
            uploadUrl,
            "PUT",
            expiresAt,
            requiredHeaders));
    }
}
