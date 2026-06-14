using CSharpFunctionalExtensions;
using Dapper;
using DS.Application.Abstractions.Handlers;
using DS.Application.Abstractions.Database;
using DS.Contracts.Common;
using DS.Contracts.Locations.Get;
using DS.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;

namespace DS.Application.Locations.Handlers.Queries.List;

public class GetLocationsHandler : IQueryHandler<PagedResult<LocationListItemDto>, GetLocationsQuery>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<GetLocationsHandler> _logger;

    private static readonly Dictionary<string, string> AllowedSortExpressions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["name"] = "lower(l.name)",
        ["address"] = "lower(l\".address_full\")",
        ["createdAt"] = "l.created_at",
        ["departmentCount"] = "dept_count"
    };

    public GetLocationsHandler(IDbConnectionFactory dbConnectionFactory, ILogger<GetLocationsHandler> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<Result<PagedResult<LocationListItemDto>, Errors>> Handle(GetLocationsQuery query, CancellationToken cancellationToken = default)
    {
        var request = query.Request;

        if (request.PageNumber < 1)
            return Result.Failure<PagedResult<LocationListItemDto>, Errors>(Error.Validation("pagination.invalid_page_number", "Page must be >= 1", "PageNumber"));

        if (request.PageSize < 1 || request.PageSize > 100)
            return Result.Failure<PagedResult<LocationListItemDto>, Errors>(Error.Validation("pagination.invalid_page_size", "PageSize must be between 1 and 100", "PageSize"));

        if (!AllowedSortExpressions.ContainsKey(request.SortBy))
            return Result.Failure<PagedResult<LocationListItemDto>, Errors>(Error.Validation("sort.invalid_field", "SortBy is invalid", "SortBy"));

        var sortExpr = AllowedSortExpressions[request.SortBy];
        var sortDir = request.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";

        var page = request.PageNumber;
        var pageSize = request.PageSize;
        var offset = (page - 1) * pageSize;

        var sql = new StringBuilder();

        sql.AppendLine("WITH filtered AS (");
        sql.AppendLine("  SELECT l.id, l.name, l.address_full AS address, l.created_at, COALESCE(count(dl.location_id),0) AS dept_count");
        sql.AppendLine("  FROM locations l");
        sql.AppendLine("  LEFT JOIN departments_locations dl ON dl.location_id = l.id");
        sql.AppendLine("  WHERE 1=1");

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            sql.AppendLine("    AND (lower(l.name) LIKE @Search OR lower(l.address_full) LIKE @Search)");
        }

        sql.AppendLine("  GROUP BY l.id");

        if (request.MinDepartmentCount.HasValue)
        {
            sql.AppendLine("  HAVING COALESCE(count(dl.location_id),0) >= @MinDepartmentCount");
        }

        sql.AppendLine(")");
        sql.AppendLine("SELECT COUNT(*) FROM filtered;");
        sql.AppendLine($"SELECT id AS Id, name AS Name, address AS Address, created_at AS CreatedAt, dept_count AS DepartmentCount FROM filtered ORDER BY {sortExpr} {sortDir} LIMIT @PageSize OFFSET @Offset;");

        try
        {
            using var conn = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken) as IDbConnection;
            var parameters = new DynamicParameters();
            parameters.Add("PageSize", pageSize, DbType.Int32);
            parameters.Add("Offset", offset, DbType.Int32);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                parameters.Add("Search", $"%{request.Search.Trim().ToLowerInvariant()}%", DbType.String);
            }

            if (request.MinDepartmentCount.HasValue)
            {
                parameters.Add("MinDepartmentCount", request.MinDepartmentCount.Value, DbType.Int32);
            }

            _logger.LogDebug("Executing SQL: {Sql}", sql.ToString());

            using var multi = await conn.QueryMultipleAsync(sql.ToString(), parameters);

            var total = await multi.ReadFirstAsync<int>();
            var items = (await multi.ReadAsync<LocationListItemDto>()).ToList();

            var paged = new PagedResult<LocationListItemDto>(items, total, page, pageSize);

            return Result.Success<PagedResult<LocationListItemDto>, Errors>(paged);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetLocations query failed");
            return Result.Failure<PagedResult<LocationListItemDto>, Errors>(Error.Failure("get.locations.failed", "Failed to get locations"));
        }
    }
}
