using DS.Domain.Models.Departments;
using DS.Infrastructure;

namespace DS.Application.Departments.Repositories;

public class DepartmentsRepository : IDepartmentsRepository
{
    private readonly DsDbContext _dbContext;

    public DepartmentsRepository(DsDbContext context)
    {
        _dbContext = context;
    }

    public async Task<Guid> CreateAsync(
        Department department,
        CancellationToken cancellationToken)
    {
        await _dbContext.Departments.AddAsync(department, cancellationToken);

        return department.Id;
    }

    public async Task<Department?> GetByIdAsync(
        Guid? id,
        CancellationToken cancellationToken)
    {
        return id is null ? null : await _dbContext.Departments.FindAsync(id);
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Guid Update(
        Department department,
        CancellationToken cancellationToken)
    {
        _dbContext.Attach(department);

        return department.Id;
    }
}
