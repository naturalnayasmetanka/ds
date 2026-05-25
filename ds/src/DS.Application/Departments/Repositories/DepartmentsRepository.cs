using DS.Domain.Models.Departments;
using DS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DS.Application.Departments.Repositories;

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

    public async Task<Guid> AddAsync(
        Department department,
        CancellationToken cancellationToken)
    {
        await _dbContext.Departments.AddAsync(department, cancellationToken);

        return department.Id;
    }

    public async Task<Department?> GetByFieldAsync(
        Expression<Func<Department, bool>> predicate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Department>().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<List<Department>> GetListByFieldAsync(
       Expression<Func<Department, bool>> predicate,
       CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Department>().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        
    }
}
