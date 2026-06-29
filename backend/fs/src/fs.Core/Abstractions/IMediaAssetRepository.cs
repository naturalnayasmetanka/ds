using CSharpFunctionalExtensions;
using FS.Core.Entities;
using FS.Core.Exceptions;
using System.Linq.Expressions;

namespace FS.Core.Abstractions;

public interface IMediaAssetRepository
{
    Task<Result<Guid, Error>> AddAsync(
        MediaAsset mediaAsset, CancellationToken cancellationToken = default);

    Task<Result<MediaAsset, Error>> GetBy(
        Expression<Func<MediaAsset, bool>> predicate, CancellationToken cancellationToken = default);

    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
