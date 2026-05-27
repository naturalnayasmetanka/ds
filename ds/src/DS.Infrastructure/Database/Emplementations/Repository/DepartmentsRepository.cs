using CSharpFunctionalExtensions;
using DS.Application.Departments.Repositories;
using DS.Domain.Exceptions;
using DS.Domain.Models.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DS.Infrastructure.Database.Emplementations.Repository;

public class DepartmentsRepository : IDepartmentsRepository
{
    private readonly DsDbContext _dbContext;
    private readonly ILogger<DepartmentsRepository> _logger;
    public DepartmentsRepository(
        DsDbContext context,
        ILogger<DepartmentsRepository> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public async Task<Result<Guid>> AddAsync(Department department, CancellationToken cancellationToken = default)
    {
        await _dbContext.Departments.AddAsync(department, cancellationToken);

        return Result.Success<Guid>(department.Id);
    }

    public async Task<Result<Department?>> GetByFieldAsync(Expression<Func<Department, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Departments.FirstOrDefaultAsync(predicate, cancellationToken);

        return Result.Success<Department?>(result);
    }

    public async Task<Result<List<Department>>> GetListByFieldAsync(Expression<Func<Department, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Departments.Where(predicate).ToListAsync(cancellationToken);

        return Result.Success<List<Department>>(result);
    }

    public async Task<UnitResult<Error>> SaveAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<Error>();
    }

}
