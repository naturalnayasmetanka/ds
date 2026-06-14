using DS.Contracts.Common;
using DS.Domain.Models.Departments;

namespace DS.Application.Extentions;

public static class QueryableExtentions
{
   
    public static IQueryable<T> WithPaginationAndSorting<T>(
        this IQueryable<T> query,
        IPaginatedRequest request,
        Func<IQueryable<T>, string, bool, IOrderedQueryable<T>> sortFieldProvider)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(sortFieldProvider);

        var isDescending = request.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);
       
        var sortedQuery = sortFieldProvider(query, request.SortBy.ToLower(), isDescending);

        var skip = (request.PageNumber - 1) * request.PageSize;
        return sortedQuery
            .Skip(skip)
            .Take(request.PageSize);
    }
}

