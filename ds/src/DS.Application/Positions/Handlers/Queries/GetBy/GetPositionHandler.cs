using CSharpFunctionalExtensions;
using DS.Application.Abstractions.Database;
using DS.Application.Abstractions.Handlers;
using DS.Contracts.Positions.GetById;
using DS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DS.Contracts.Positions.GetById;

namespace DS.Application.Positions.Handlers.Queries.GetBy;

public class GetPositionHandler : IQueryHandler<GetPositionByIdResponse?, GetPositionQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetPositionHandler> _logger;

    public GetPositionHandler(IReadDbContext readDbContext, ILogger<GetPositionHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<GetPositionByIdResponse?, Errors>> Handle(GetPositionQuery query, CancellationToken cancellationToken = default)
    {
        var position = await _readDbContext.PositionsRead.FirstOrDefaultAsync(p => p.Id == query.request.Id && p.IsActive, cancellationToken);

        if (position is null)
            return Result.Failure<GetPositionByIdResponse?, Errors>(Error.NotFound("position.not.found", "Должность не найдена", query.request.Id));

        // Map to response (simple placeholder)
        var resp = new GetPositionByIdResponse();

        return Result.Success<GetPositionByIdResponse?, Errors>(resp);
    }
}
