using DS.Contracts.Positions.GetById;
using DS.Application.Abstractions.Handlers;

namespace DS.Application.Positions.Handlers.Queries.GetBy;

public record GetPositionQuery(GetPositionByIdRequest request) : IQuery;
