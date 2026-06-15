using DS.Contracts.Positions.Get;
using DS.Application.Abstractions.Handlers;

namespace DS.Application.Positions.Handlers.Queries.GetList;

public record GetPositionsQuery(GetPositionsRequest Request) : IQuery;
