using DS.Domain.Models.Departments;
using DS.Domain.Models.Locations;

namespace DS.Application.Abstractions.Database;

public interface IReadDbContext
{
    IQueryable<Department> DepartmentsRead { get; }
    IQueryable<Location> LocationsRead { get; }
}
