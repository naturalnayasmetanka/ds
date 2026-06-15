using DS.Contracts.Common;

namespace DS.Contracts.Locations.Get;

public record GetLocationsRequest : IPaginatedRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string SortBy { get; init; } = "name";
    public string SortDirection { get; init; } = "asc";
    public string? Search { get; init; }
    public int? MinDepartmentCount { get; init; }
}
