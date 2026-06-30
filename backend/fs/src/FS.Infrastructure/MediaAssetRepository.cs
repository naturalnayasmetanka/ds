using CSharpFunctionalExtensions;
using FS.Core.Abstractions;
using FS.Core.Entities;
using FS.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;

namespace FS.Infrastructure.Postgres;

public class MediaAssetRepository : IMediaAssetRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<MediaAssetRepository> _logger;
    public MediaAssetRepository(
        AppDbContext context, ILogger<MediaAssetRepository> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> AddAsync(
        MediaAsset mediaAsset, CancellationToken cancellationToken = default)
    {
        await _dbContext.MediaAssets.AddAsync(mediaAsset, cancellationToken);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);

            return mediaAsset.Id;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            return Result.Failure<Guid, Error>(Error.Failure("add.error", "Error mediaasset"));
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was cancelled while creating mediaAsset");

            return Result.Failure<Guid, Error>(Error.Failure("add.error", "Operation was cancelled while creating mediaAsset"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating mediaAsset");

            return Result.Failure<Guid, Error>(Error.Failure("add.error", "Unexpected error while creating mediaAsset"));
        }
    }

    public async Task<Result<MediaAsset, Error>> GetBy(Expression<Func<MediaAsset, bool>> predicate, CancellationToken cancellationToken = default)
    {
        MediaAsset? asset = await _dbContext.MediaAssets.FirstOrDefaultAsync(predicate, cancellationToken);

        if (asset is null)
            return Result.Failure<MediaAsset, Error>(Error.NotFound("mediaasset.notfound", "Media asset not found", null));

        return Result.Success<MediaAsset, Error>(asset);
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
