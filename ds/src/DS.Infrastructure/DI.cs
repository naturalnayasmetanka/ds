using DS.Application.Abstractions.Database;
using DS.Application.Departments.Repositories;
using DS.Application.DepartmentsLocations.Repositories;
using DS.Application.DepartmentsPositions.Repositories;
using DS.Application.Locations.Repositories;
using DS.Application.Positions.Repositories;
using DS.Infrastructure.Database.Abstractions;
using DS.Infrastructure.Database.Emplementations;
using DS.Infrastructure.Database.Emplementations.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastractureDb(this IServiceCollection services)
    {
        services.AddScoped<DsDbContext>();

        services.AddSingleton<IDbConnectionFactory, NpgSqlConnectionFactory>();

        services.AddScoped<ITransactionManager, TransactionManager>();

        //services.AddScoped<ILocationsRepository, EfCoreLocationsRepository>();
        //services.AddScoped<ILocationsRepository, NpgSqlLocationsRepository>();

        services.AddScoped<ILocationsRepository, LocationsRepository>();
        services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();
        services.AddScoped<IDepartmentsLocationsRepository, DepartmentsLocationsRepository>();

        services.AddScoped<IPositionsRepository, PositionsRepository>();
        services.AddScoped<IDepartmentsPositionsRepository, DepartmentsPositionsRepository>();

        return services;
    }
}
