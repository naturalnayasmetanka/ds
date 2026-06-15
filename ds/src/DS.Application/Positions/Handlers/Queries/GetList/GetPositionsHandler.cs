using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Extentions;
using DS.Contracts.Common;
using DS.Contracts.Positions.Get;
using DS.Contracts.Positions;
using DS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DS.Application.Positions.Handlers.Queries.GetList;

public class GetPositionsHandler : IQueryHandler<PagedResult<PositionListItemDto>, GetPositionsQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetPositionsHandler> _logger;

    public GetPositionsHandler(IReadDbContext readDbContext, ILogger<GetPositionsHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<PagedResult<PositionListItemDto>, Errors>> Handle(GetPositionsQuery query, CancellationToken cancellationToken = default)
    {
        var request = query.Request;

        var page = request?.PageNumber ?? 1;
        var pageSize = request?.PageSize ?? 50;

        var baseQuery = _readDbContext.PositionsRead.Where(p => p.IsActive);

        var countTask = baseQuery.CountAsync(cancellationToken);

        var itemsTask = baseQuery
            .Select(p => new PositionListItemDto(p.Id, p.Name, p.CreateAt, p.UpdateAt))
            .ToListAsync(cancellationToken);

        await Task.WhenAll(countTask, itemsTask).ConfigureAwait(false);

        var totalCount = await countTask;
        var items = await itemsTask;

        var pagedResult = new PagedResult<PositionListItemDto>(items, totalCount, page, pageSize);

        return Result.Success<PagedResult<PositionListItemDto>, Errors>(pagedResult);
    }
}
