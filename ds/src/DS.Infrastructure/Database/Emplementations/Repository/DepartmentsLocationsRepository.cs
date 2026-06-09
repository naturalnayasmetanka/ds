using CSharpFunctionalExtensions;
using DS.Application.DepartmentsLocations.Repositories;
using DS.Domain.Exceptions;
using DS.Domain.Models.DepartmentsLocations;
using Microsoft.Extensions.Logging;

namespace DS.Infrastructure.Database.Emplementations.Repository;

public class DepartmentsLocationsRepository : IDepartmentsLocationsRepository
{
    private readonly DsDbContext _dbContext;
    private readonly ILogger<DepartmentsLocationsRepository> _logger;

    public DepartmentsLocationsRepository(
        DsDbContext dsDbContext,
        ILogger<DepartmentsLocationsRepository> logger)
    {
        _dbContext = dsDbContext;
        _logger = logger;
    }

    public async Task<Result<DepartmentLocation>> BindAsync(DepartmentLocation departmentLocation, CancellationToken cancellationToken)
    {
        await _dbContext.DepartmentsLocations.AddAsync(departmentLocation, cancellationToken);

        return Result.Success<DepartmentLocation>(departmentLocation);
    }

    public UnitResult<Error> Unbind(DepartmentLocation departmentLocation, CancellationToken cancellationToken)
    {
        _dbContext.DepartmentsLocations.Remove(departmentLocation);

        return new UnitResult<Error>();
    }

    public async Task<UnitResult<Error>> CreateAsync(DepartmentLocation departmentLocation, CancellationToken cancellationToken)
    {
        await _dbContext.DepartmentsLocations.AddAsync(departmentLocation, cancellationToken);

        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> AddRangeAsync(List<DepartmentLocation> departmentLocation, CancellationToken cancellationToken)
    {
        await _dbContext.DepartmentsLocations.AddRangeAsync(departmentLocation, cancellationToken);

        return UnitResult.Success<Error>();
    }

    public async Task<Result<DepartmentLocation?>> GetByIdsAsync(DepartmentLocation departmentLocation, CancellationToken cancellationToken)
    {
        var result = await _dbContext.DepartmentsLocations.FindAsync(departmentLocation.DepartmentId, departmentLocation.LocationId, cancellationToken);

        return Result.Success<DepartmentLocation?>(result);
    }
}