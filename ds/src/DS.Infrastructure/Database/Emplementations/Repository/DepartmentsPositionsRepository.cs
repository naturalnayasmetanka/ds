using CSharpFunctionalExtensions;
using DS.Application.DepartmentsPositions.Repositories;
using DS.Domain.Models.DepartmentsPositions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DS.Infrastructure.Database.Emplementations.Repository;

public class DepartmentsPositionsRepository : IDepartmentsPositionsRepository
{
    private readonly DsDbContext _dbContext;
    private readonly ILogger<DepartmentsPositionsRepository> _logger;

    public DepartmentsPositionsRepository(
        DsDbContext dbContext,
        ILogger<DepartmentsPositionsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<DepartmentPosition>> AddAsync(DepartmentPosition newDepartmentPosition, CancellationToken cancellationToken = default)
    {
        await _dbContext.DepartmentsPositions.AddAsync(newDepartmentPosition, cancellationToken);

        return Result.Success<DepartmentPosition>(newDepartmentPosition);
    }

    public async Task<Result<DepartmentPosition?>> GetByFieldAsync(Expression<Func<DepartmentPosition, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.DepartmentsPositions.FirstOrDefaultAsync(predicate, cancellationToken);

        return Result.Success<DepartmentPosition?>(result);
    }

    public async Task<Result<List<DepartmentPosition>>> GetListByFieldAsync(Expression<Func<DepartmentPosition, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.DepartmentsPositions.Where(predicate).ToListAsync(cancellationToken);

        return Result.Success<List<DepartmentPosition>>(result);
    }

    public async Task<Result<bool>> RemoveAsync(DepartmentPosition departmentPosition, CancellationToken cancellationToken = default)
    {
        _dbContext.DepartmentsPositions.Remove(departmentPosition);

        return Result.Success<bool>(true);
    }
}
