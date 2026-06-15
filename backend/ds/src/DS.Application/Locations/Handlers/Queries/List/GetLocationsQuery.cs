using DS.Application.Abstractions.Handlers;
using DS.Contracts.Locations.Get;

namespace DS.Application.Locations.Handlers.Queries.List;

public record GetLocationsQuery(GetLocationsRequest Request) : IQuery;
