using DS.Application.Abstractions.Handlers;
using DS.Application.Locations.Handlers.Create;
using DS.Application.Locations.Handlers.Delete;
using DS.Application.Locations.Handlers.Update;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Application.Locations;

public static class DI
{
    public static IServiceCollection AddLocations(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddValidatorsFromAssembly(typeof(DI).Assembly);

        services.AddScoped<ICommandHandler<Guid, CreateLocationCommand>, CreateLocationHandler>();
        services.AddScoped<ICommandHandler<Guid, UpdateLocationCommand>, UpdateLocationHandler>();
        services.AddScoped<ICommandHandler<Guid, DeleteLocationCommand>, DeleteLocationHandler>();

        return services;
    }
}
