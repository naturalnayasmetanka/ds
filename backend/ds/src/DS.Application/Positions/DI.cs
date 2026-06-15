using DS.Application.Abstractions.Handlers;
using DS.Application.Positions.Handlers.Commands.Create;
using DS.Application.Positions.Handlers.Commands.Delete;
using DS.Application.Positions.Handlers.Commands.Update;
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
