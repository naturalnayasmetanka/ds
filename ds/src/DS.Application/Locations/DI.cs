using DS.Application.Locations.Repositories;
using DS.Application.Locations.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Application.Locations;

public static class DI
{
    public static IServiceCollection AddLocations(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddValidatorsFromAssembly(typeof(DI).Assembly);

        services.AddScoped<ILocationsRepository, LocationsRepository>();
        services.AddScoped<ILocationsService, LocationsService>();

        return services;
    }
}
