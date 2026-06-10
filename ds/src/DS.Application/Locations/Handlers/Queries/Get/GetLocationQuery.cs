using DS.Application.Abstractions.Handlers;
using DS.Contracts.Locations.Get;
using DS.Contracts.Locations.GetById;

namespace DS.Application.Locations.Handlers.Queries.Get;

public record GetLocationQuery(GetLocationRequest request) : IQuery;
