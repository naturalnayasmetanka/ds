using DS.Application.Abstractions.Handlers;
using DS.Application.DepartmentsPositions.Handlers.Bind;
using DS.Application.DepartmentsPositions.Handlers.Unbind;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Application.DepartmentsPositions;

public static class DI
{
    public static IServiceCollection AddDepartmentsPositions(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ICommandHandler<BindDepartmentPositionCommand>, BindDepartmentPositionHandler>();
        services.AddScoped<ICommandHandler<UnbindDepartmentPositionCommand>, UnbindDepartmentPositionHandler>();

        return services;
    }
}
