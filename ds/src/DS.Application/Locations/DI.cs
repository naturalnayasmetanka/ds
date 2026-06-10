using DS.Application.Abstractions.Handlers;
using DS.Application.Locations.Handlers.Commands.Create;
using DS.Application.Locations.Handlers.Commands.Delete;
using DS.Application.Locations.Handlers.Commands.Update;
using DS.Application.Locations.Handlers.Queries.Get;
using DS.Contracts.Locations.GetById;
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

        services.AddScoped<IQueryHandler<GetLocationResponse?, GetLocationQuery>, GetLocationHandler>();

        return services;
    }
}
