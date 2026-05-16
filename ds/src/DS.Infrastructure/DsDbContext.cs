using DS.Domain.Models.Departments;
using DS.Domain.Models.Locations;
using DS.Domain.Models.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DS.Infrastructure;

public class DsDbContext : DbContext
{
    private const string DS_CONNECTION_STRING = "DsDbConnection";

    private readonly IConfiguration _configuration;

    public DsDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Position> Positions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(DS_CONNECTION_STRING));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DsDbContext).Assembly);
    }
}
