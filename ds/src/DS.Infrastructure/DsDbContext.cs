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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(DS_CONNECTION_STRING));
    }
}
