using DS.Application.Abstractions.Handlers;
using DS.Application.Locations.Handlers.Commands.Create;
using DS.Application.Locations.Handlers.Commands.Delete;
using DS.Application.Locations.Handlers.Commands.Update;
using DS.Application.Locations.Handlers.Queries.Get;
using DS.Application.Locations.Handlers.Queries.GetTop;
using DS.Application.Locations.Handlers.Queries.List;
using DS.Contracts.Locations.GetById;
using DS.Contracts.Locations.GetTop;
using DS.Contracts.Locations.Get;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using DS.Contracts.Common;

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
        services.AddScoped<IQueryHandler<List<GetTopResponse>, UnitQuery>, GetTopHandler>();
        services.AddScoped<IQueryHandler<PagedResult<LocationListItemDto>, GetLocationsQuery>, GetLocationsHandler>();

        return services;
    }
}
