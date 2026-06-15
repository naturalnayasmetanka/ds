using DS.Contracts.Common;

namespace DS.Contracts.Departments.Get;

public record GetDepartmentsListRequest : IPaginatedRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string SortBy { get; init; } = "dateFrom";
    public string SortDirection { get; init; } = "desc";
    public string? Search { get; init; }
    public bool OnlyActive { get; init; } = true;
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
}
