namespace DS.Contracts.Common;

public interface IPaginatedRequest
{
    int PageNumber { get; }
    int PageSize { get; }
    string SortBy { get; }
    string SortDirection { get; }
}
