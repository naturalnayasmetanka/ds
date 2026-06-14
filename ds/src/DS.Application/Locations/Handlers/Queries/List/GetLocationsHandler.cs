using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Handlers;
using DS.Application.Abstractions.Database;
using DS.Application.Extentions;
using DS.Contracts.Common;
using DS.Contracts.Locations.Get;
using DS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DS.Application.Locations.Handlers.Queries.List;

public class GetLocationsHandler : IQueryHandler<PagedResult<LocationListItemDto>, GetLocationsQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetLocationsHandler> _logger;

    private static readonly HashSet<string> AllowedSortFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "name",
        "address",
        "createdAt",
        "departmentCount"
    };

    public GetLocationsHandler(IReadDbContext readDbContext, ILogger<GetLocationsHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    private sealed record LocAgg
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public int DepartmentCount { get; init; }
    }

    public async Task<Result<PagedResult<LocationListItemDto>, Errors>> Handle(GetLocationsQuery query, CancellationToken cancellationToken = default)
    {
        var request = query.Request;

        if (request.PageNumber < 1)
            return Result.Failure<PagedResult<LocationListItemDto>, Errors>(Error.Validation("pagination.invalid_page_number", "Номер страницы должен быть >= 1", "PageNumber"));

        if (request.PageSize < 1 || request.PageSize > 100)
            return Result.Failure<PagedResult<LocationListItemDto>, Errors>(Error.Validation("pagination.invalid_page_size", "Размер страницы должен быть от 1 до 100", "PageSize"));

        if (!AllowedSortFields.Contains(request.SortBy))
            return Result.Failure<PagedResult<LocationListItemDto>, Errors>(Error.Validation("sort.invalid_field", $"Поле для сортировки должно быть одним из: {string.Join(", ", AllowedSortFields)}", "SortBy"));

        var AllowedSortDirections = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "asc", "desc" };

        if (!AllowedSortDirections.Contains(request.SortDirection))
            return Result.Failure<PagedResult<LocationListItemDto>, Errors>(Error.Validation("sort.invalid_direction", "Направление сортировки должно быть 'asc' или 'desc'", "SortDirection"));

        var page = request.PageNumber;
        var pageSize = request.PageSize;

        var baseQuery = _readDbContext.LocationsRead;

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchTerm = request.Search.ToLower().Trim();
            baseQuery = baseQuery.Where(l => l.Name.Value.ToLower().Contains(searchTerm) || l.Address.FullAddress.ToLower().Contains(searchTerm));
        }

        if (request.MinDepartmentCount.HasValue)
        {
            var minCount = request.MinDepartmentCount.Value;
            baseQuery = baseQuery.Where(l => _readDbContext.DepartmentsLocationsRead.Count(dl => dl.LocationId == l.Id) >= minCount);
        }

        var sortedAndPagedQuery = baseQuery.WithPaginationAndSorting(
            request,
            (queryable, sortField, isDesc) => sortField switch
            {
                "name" => isDesc ? queryable.OrderByDescending(l => l.Name.Value) : queryable.OrderBy(l => l.Name.Value),
                "address" => isDesc ? queryable.OrderByDescending(l => l.Address.FullAddress) : queryable.OrderBy(l => l.Address.FullAddress),
                "createdat" => isDesc ? queryable.OrderByDescending(l => l.CreatedAt) : queryable.OrderBy(l => l.CreatedAt),
                "departmentcount" => isDesc ? queryable.OrderByDescending(l => _readDbContext.DepartmentsLocationsRead.Count(dl => dl.LocationId == l.Id)) : queryable.OrderBy(l => _readDbContext.DepartmentsLocationsRead.Count(dl => dl.LocationId == l.Id)),
                _ => isDesc ? queryable.OrderByDescending(l => l.Name.Value) : queryable.OrderBy(l => l.Name.Value)
            });

        var countTask = baseQuery.CountAsync(cancellationToken);

        var itemsTask = sortedAndPagedQuery
            .Select(l => new LocationListItemDto(
                l.Id,
                l.Name.Value,
                l.Address.FullAddress,
                l.CreatedAt,
                _readDbContext.DepartmentsLocationsRead.Count(dl => dl.LocationId == l.Id)))
            .ToListAsync(cancellationToken);

        await Task.WhenAll(countTask, itemsTask).ConfigureAwait(false);

        var totalCount = await countTask;
        var items = await itemsTask;

        var pagedResult = new PagedResult<LocationListItemDto>(
            Items: items,
            TotalCount: totalCount,
            PageNumber: request.PageNumber,
            PageSize: request.PageSize);

        return Result.Success<PagedResult<LocationListItemDto>, Errors>(pagedResult);
    }
}
