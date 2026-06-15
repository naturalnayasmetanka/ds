using System.Collections.Generic;
using DS.Contracts.Positions.Get;

namespace DS.Contracts.Positions.Get;

public record GetPositionsResponse(IEnumerable<PositionListItemDto> Items, int TotalCount);
