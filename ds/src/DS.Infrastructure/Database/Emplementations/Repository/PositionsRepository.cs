using CSharpFunctionalExtensions;
using DS.Application.Positions.Repositories;
using DS.Domain.Models.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DS.Infrastructure.Database.Emplementations.Repository;

public class PositionsRepository : IPositionsRepository
{
    private readonly DsDbContext _dbContext;
    private readonly ILogger<PositionsRepository> _logger;

    public PositionsRepository(
        DsDbContext dbContext,
        ILogger<PositionsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Guid>> AddAsync(Position newPosition, CancellationToken cancellationToken = default)
    {
        await _dbContext.Positions.AddAsync(newPosition, cancellationToken);

        return Result.Success<Guid>(newPosition.Id);
    }

    public async Task<Result<Position?>> GetByFieldAsync(Expression<Func<Position, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Positions.FirstOrDefaultAsync(predicate, cancellationToken);

        return Result.Success<Position?>(result);
    }

    public async Task<Result<List<Position>>> GetListByFieldAsync(Expression<Func<Position, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Positions.Where(predicate).ToListAsync(cancellationToken);

        return Result.Success<List<Position>>(result);
    }
}
