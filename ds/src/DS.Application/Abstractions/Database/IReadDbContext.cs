using DS.Domain.Models.Departments;
using DS.Domain.Models.Locations;
using DS.Domain.Models.DepartmentsLocations;
using DS.Domain.Models.Positions;

namespace DS.Application.Abstractions.Database;

public interface IReadDbContext
{
    IQueryable<Department> DepartmentsRead { get; }
    IQueryable<Location> LocationsRead { get; }
    IQueryable<DepartmentLocation> DepartmentsLocationsRead { get; }
    IQueryable<Position> PositionsRead { get; }
}
