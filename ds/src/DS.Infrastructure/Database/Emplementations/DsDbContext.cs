using DS.Domain.Models.Departments;
using DS.Domain.Models.Locations;
using DS.Domain.Models.Positions;
using DS.Infrastructure.Database.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DS.Infrastructure.Database.Emplementations;

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

        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DsDbContext).Assembly);
    }

    private ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
    }
}
