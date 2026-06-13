using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Contracts.Locations.GetTop;
using DS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DS.Application.Locations.Handlers.Queries.GetTop;

public class GetTopHandler : IQueryHandler<List<GetTopResponse>, UnitQuery>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<GetTopHandler> _logger;
    private readonly IReadDbContext _readDbContext;
    public GetTopHandler(
        IDbConnectionFactory dbConnectionFactory,
        IReadDbContext readDbContext,
        ILogger<GetTopHandler> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<List<GetTopResponse>, Errors>> Handle(
        UnitQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await (from dl in _readDbContext.DepartmentsLocationsRead
                            group dl by dl.LocationId into g
                            join l in _readDbContext.LocationsRead on g.Key equals l.Id
                            orderby g.Count() descending
                            select new GetTopResponse
                            {
                                Id = l.Id,
                                Name = l.Name.Value,
                                Address = l.Address.FullAddress,
                                DepartmentCount = g.Count()
                            })
                           .Take(5)
                           .ToListAsync(cancellationToken);

        if (result.Count == 0)
            return Result.Failure<List<GetTopResponse>, Errors>(Error.NotFound("location.not.found", "Локация c подразделениями не найдена", Guid.Empty));

        return Result.Success<List<GetTopResponse>, Errors>(result);
    }
}
