using DS.Application.Abstractions;
using DS.Application.DepartmentsLocations.Handlers.Bind;
using DS.Application.DepartmentsLocations.Handlers.Unbind;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Application.DepartmentsLocations;

public static class DI
{
    public static IServiceCollection AddDepartmentsLocations(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ICommandHandler<BindDepartmentLocationCommand>, BindDepartmentLocationHandler>();
        services.AddScoped<ICommandHandler<UnBindDepartmentLocationCommand>, UnBindDepartmentLocationHandler>();

        return services;
    }
}
