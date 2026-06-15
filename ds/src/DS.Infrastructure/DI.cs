using DS.Application.Abstractions.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using DS.Application.Departments.Repositories;
using DS.Application.DepartmentsLocations.Repositories;
using DS.Application.DepartmentsPositions.Repositories;
using DS.Application.Locations.Repositories;
using DS.Infrastructure.Background;
using DS.Application.Positions.Repositories;
using DS.Infrastructure.Database.Emplementations;
using DS.Infrastructure.Database.Emplementations.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastractureDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DsDbContext>();
        services.AddScoped<IReadDbContext, DsDbContext>();

        services.AddSingleton<IDbConnectionFactory, NpgSqlConnectionFactory>();

        services.AddScoped<ITransactionManager, TransactionManager>();

        //services.AddScoped<ILocationsRepository, EfCoreLocationsRepository>();
        //services.AddScoped<ILocationsRepository, NpgSqlLocationsRepository>();

        services.AddScoped<ILocationsRepository, LocationsRepository>();
        services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();
        services.AddScoped<IDepartmentsLocationsRepository, DepartmentsLocationsRepository>();

        services.AddScoped<IPositionsRepository, PositionsRepository>();
        services.AddScoped<IDepartmentsPositionsRepository, DepartmentsPositionsRepository>();

        // configure cleanup options and hosted background cleanup service
        services.Configure<CleanupOptions>(configuration.GetSection("Cleanup"));
        services.AddHostedService<CleanupBackgroundService>();

        return services;
    }
}
