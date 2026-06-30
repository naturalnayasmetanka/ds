using CSharpFunctionalExtensions;
using FS.Contracts;
using FS.Core.Abstractions;
using FS.Core.Abstractions.Common;
using FS.Core.Enums;
using FS.Core.Exceptions;
using FS.Core.Options;
using Microsoft.Extensions.Options;

namespace FS.Core.Features;

public record GetMediaAssetUrlQuery(Guid MediaAssetId) : IQuery;

/// <summary>
/// Минимальная проверка получения файла: для READY asset'а выдаём свежий
/// presigned GET URL, для остальных статусов — доменную ошибку без URL.
/// </summary>
public sealed class GetMediaAssetUrlHandler : IQueryHandler<GetMediaAssetUrlResponse, GetMediaAssetUrlQuery>
{
    private readonly IS3Provider _s3Provider;
    private readonly IMediaAssetRepository _mediaAssetRepository;
    private readonly IOptions<S3Options> _s3Options;

    public GetMediaAssetUrlHandler(
        IS3Provider s3Provider,
        IMediaAssetRepository mediaAssetRepository,
        IOptions<S3Options> s3Options)
    {
        _s3Provider = s3Provider;
        _mediaAssetRepository = mediaAssetRepository;
        _s3Options = s3Options;
    }

    public async Task<Result<GetMediaAssetUrlResponse, Errors>> Handle(
        GetMediaAssetUrlQuery query, CancellationToken cancellationToken = default)
    {
        var assetResult = await _mediaAssetRepository.GetBy(
            m => m.Id == query.MediaAssetId && m.MediaStatus != MediaStatus.DELETED,
            cancellationToken);

        if (assetResult.IsFailure)
            return Result.Failure<GetMediaAssetUrlResponse, Errors>(assetResult.Error);

        var asset = assetResult.Value;

        if (asset.MediaStatus != MediaStatus.READY)
            return Result.Failure<GetMediaAssetUrlResponse, Errors>(
                Error.Conflict("asset.not.ready", $"Asset is not ready yet, current status is {asset.MediaStatus}"));

        var url = await _s3Provider.GenerateDownloadUrl(asset.Key);

        return Result.Success<GetMediaAssetUrlResponse, Errors>(new GetMediaAssetUrlResponse(
            asset.Id,
            url,
            DateTime.UtcNow.AddHours(_s3Options.Value.DownloadUrlExpirationHours)));
    }
}
