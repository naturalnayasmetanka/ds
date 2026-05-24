using DS.Domain.Models.DepartmentsLocations;
using DS.Infrastructure;

namespace DS.Application.DepartmentsLocations.Repositories;

public class DepartmentsLocationsRepository : IDepartmentsLocationsRepository
{
    private readonly DsDbContext _dbContext;
    public DepartmentsLocationsRepository(DsDbContext dsDbContext)
    {
        _dbContext = dsDbContext;
    }

    public async Task<DepartmentLocation> BindAsync(
        DepartmentLocation departmentLocation,
        CancellationToken cancellationToken)
    {
        await _dbContext.DepartmentsLocations.AddAsync(departmentLocation, cancellationToken);

        return departmentLocation;
    }

    public void UnbindAsync(
        DepartmentLocation departmentLocation,
        CancellationToken cancellationToken)
    {
        _dbContext.DepartmentsLocations.Remove(departmentLocation);
    }

    public async Task CreateAsync(
        DepartmentLocation departmentLocation,
        CancellationToken cancellationToken)
    {
        await _dbContext.DepartmentsLocations.AddAsync(departmentLocation, cancellationToken);
    }

    public async Task CreateRangeAsync(
        List<DepartmentLocation> departmentLocation,
        CancellationToken cancellationToken)
    {
        await _dbContext.DepartmentsLocations.AddRangeAsync(departmentLocation, cancellationToken);
    }

    public async Task<DepartmentLocation?> GetByIdsAsync(
        DepartmentLocation departmentLocation,
        CancellationToken cancellationToken)
    {
        return await _dbContext.DepartmentsLocations
             .FindAsync(departmentLocation.DepartmentId, departmentLocation.LocationId, cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync();
    }

}