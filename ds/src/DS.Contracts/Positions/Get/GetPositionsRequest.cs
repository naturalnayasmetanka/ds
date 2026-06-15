using DS.Contracts.Common;

namespace DS.Contracts.Positions.Get;

public record GetPositionsRequest : IPaginatedRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string SortBy { get; init; } = "createdAt";
    public string SortDirection { get; init; } = "desc";
    public string? Search { get; init; }
}
