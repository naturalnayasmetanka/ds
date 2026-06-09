using DS.Application.Abstractions.Handlers;
using DS.Application.Positions.Handlers.Create;
using DS.Application.Positions.Handlers.Delete;
using DS.Application.Positions.Handlers.Update;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Application.Positions;

public static class DI
{
    public static IServiceCollection AddPositions(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ICommandHandler<Guid, CreatePositionCommand>, CreatePositionHandler>();
        services.AddScoped<ICommandHandler<Guid, UpdatePositionCommand>, UpdatePositionHandler>();
        services.AddScoped<ICommandHandler<Guid, DeletePositionCommand>, DeletePositionHandler>();

        return services;
    }
}
