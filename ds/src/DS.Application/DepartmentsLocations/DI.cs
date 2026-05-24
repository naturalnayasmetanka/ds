using DS.Application.DepartmentsLocations.Repositories;
using DS.Application.DepartmentsLocations.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Application.DepartmentsLocations;

public static class DI
{
    public static IServiceCollection AddDepartmentsLocations(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IDepartmentsLocationsRepository, DepartmentsLocationsRepository>();
        services.AddScoped<IDepartmentLocationsService, DepartmentLocationsService>();

        return services;
    }
}
