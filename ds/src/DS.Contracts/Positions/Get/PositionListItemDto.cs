using System;

namespace DS.Contracts.Positions.Get;

public record PositionListItemDto(Guid Id, string Name, DateTime CreatedAt, DateTime UpdatedAt);
