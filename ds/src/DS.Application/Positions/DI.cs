using DS.Application.Abstractions.Handlers;
using DS.Application.Positions.Handlers.Commands.Create;
using DS.Application.Positions.Handlers.Commands.Delete;
using DS.Application.Positions.Handlers.Commands.Update;
using DS.Application.Positions.Handlers.Queries.GetBy;
using DS.Application.Positions.Handlers.Queries.GetList;
using DS.Contracts.Positions.GetById;
using DS.Contracts.Positions.Get;
using DS.Contracts.Common;
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
        services.AddScoped<IQueryHandler<GetPositionByIdResponse?, GetPositionQuery>, GetPositionHandler>();
        services.AddScoped<IQueryHandler<PagedResult<PositionListItemDto>, GetPositionsQuery>, GetPositionsHandler>();

        return services;
    }
}
