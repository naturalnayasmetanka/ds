using DS.Application.Locations.Repositories;
using DS.Infrastructure.Database.Abstractions;
using DS.Infrastructure.Database.Emplementations;
using DS.Infrastructure.Database.Emplementations.Repository;

namespace DS.Presentation.DI;

public static class InfrastructureDI
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, NpgSqlConnectionFactory>();
        services.AddScoped<IDsDbContext, DsDbContext>();

        services.AddScoped<ILocationsRepository, EfCoreLocationsRepository>();

        // services.AddScoped<ILocationsRepository, NpgSqlLocationsRepository>();
        return services;
    }
}
