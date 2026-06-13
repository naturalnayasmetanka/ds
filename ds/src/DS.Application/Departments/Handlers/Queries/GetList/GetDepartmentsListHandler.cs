using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Application.Extentions;
using DS.Contracts.Departments.Get;
using DS.Contracts.Common;
using DS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DS.Application.Departments.Handlers.Queries.GetList;

public class GetDepartmentsListHandler : IQueryHandler<PagedResult<DepartmentListItemDto>, GetDepartmentsListQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetDepartmentsListHandler> _logger;

    private static readonly HashSet<string> AllowedSortFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "name",
        "path",
        "dateFrom",
        "dateTo",
        "dateTo",
        "identifier"
    };

    private static readonly HashSet<string> AllowedSortDirections = new(StringComparer.OrdinalIgnoreCase)
    {
        "asc",
        "desc"
    };

    public GetDepartmentsListHandler(
        IReadDbContext readDbContext,
        ILogger<GetDepartmentsListHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<PagedResult<DepartmentListItemDto>, Errors>> Handle(
        GetDepartmentsListQuery query,
        CancellationToken cancellationToken = default)
    {
        var request = query.Request;

        if (request.PageNumber < 1)
            return Result.Failure<PagedResult<DepartmentListItemDto>, Errors>(Error.Validation("pagination.invalid_page_number", "Номер страницы должен быть >= 1", "PageNumber"));

        if (request.PageSize < 1 || request.PageSize > 100)
            return Result.Failure<PagedResult<DepartmentListItemDto>, Errors>(Error.Validation("pagination.invalid_page_size", "Размер страницы должен быть от 1 до 100", "PageSize"));

        if (!AllowedSortFields.Contains(request.SortBy))
            return Result.Failure<PagedResult<DepartmentListItemDto>, Errors>(Error.Validation("sort.invalid_field", $"Поле для сортировки должно быть одним из: {string.Join(", ", AllowedSortFields)}", "SortBy"));

        if (!AllowedSortDirections.Contains(request.SortDirection))
            return Result.Failure<PagedResult<DepartmentListItemDto>, Errors>(Error.Validation("sort.invalid_direction", "Направление сортировки должно быть 'asc' или 'desc'", "SortDirection"));

        var baseQuery = _readDbContext.DepartmentsRead.AsNoTracking();

        if (request.OnlyActive)
            baseQuery = baseQuery.Where(d => d.IsActive);


        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchTerm = request.Search.ToLower().Trim();
            baseQuery = baseQuery.Where(d =>
                d.Name.Value.ToLower().Contains(searchTerm) ||
                d.Identifier.Value.ToLower().Contains(searchTerm));
        }

        if (request.DateFrom.HasValue)
            baseQuery = baseQuery.Where(d => d.CreatedAt >= request.DateFrom.Value);

        if (request.DateTo.HasValue)
            baseQuery = baseQuery.Where(d => d.CreatedAt <= request.DateTo.Value);

        var sortedAndPagedQuery = baseQuery.WithPaginationAndSorting(
            request,
            (query, sortField, isDescending) => sortField switch
            {
                "name" => isDescending 
                    ? query.OrderByDescending(d => d.Name.Value)
                    : query.OrderBy(d => d.Name.Value),

                "path" => isDescending
                    ? query.OrderByDescending(d => d.Path.Value)
                    : query.OrderBy(d => d.Path.Value),

                "identifier" => isDescending
                    ? query.OrderByDescending(d => d.Identifier.Value)
                    : query.OrderBy(d => d.Identifier.Value),

                "datefrom" => isDescending
                    ? query.OrderByDescending(d => d.CreatedAt)
                    : query.OrderBy(d => d.CreatedAt),

                "dateto" => isDescending
                    ? query.OrderByDescending(d => d.UpdatedAt)
                    : query.OrderBy(d => d.UpdatedAt),

                _ => query.OrderByDescending(d => d.CreatedAt)
            });

        var countTask = baseQuery.CountAsync(cancellationToken);

        var itemsTask = sortedAndPagedQuery
            .Select(d => new DepartmentListItemDto(
                d.Id,
                d.Name.Value,
                d.Path.Value,
                d.CreatedAt,
                d.UpdatedAt))
            .ToListAsync(cancellationToken);

        await Task.WhenAll(countTask, itemsTask).ConfigureAwait(false);

        var totalCount = countTask.Result;
        var items = itemsTask.Result;

        var pagedResult = new PagedResult<DepartmentListItemDto>(
            Items: items,
            TotalCount: totalCount,
            PageNumber: request.PageNumber,
            PageSize: request.PageSize);

        return Result.Success<PagedResult<DepartmentListItemDto>, Errors>(pagedResult);
    }
}
