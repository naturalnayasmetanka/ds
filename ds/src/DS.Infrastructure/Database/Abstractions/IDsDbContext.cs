using DS.Domain.Models.Departments;
using DS.Domain.Models.Locations;
using DS.Domain.Models.Positions;
using Microsoft.EntityFrameworkCore;

namespace DS.Infrastructure.Database.Abstractions;

public interface IDsDbContext
{
    DbSet<Department> Departments { get; set; }
    DbSet<Location> Locations { get; set; }
    DbSet<Position> Positions { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
