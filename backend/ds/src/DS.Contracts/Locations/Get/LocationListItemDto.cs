using System;

namespace DS.Contracts.Locations.Get;

public record LocationListItemDto(
    Guid Id,
    string Name,
    string Address,
    DateTime CreatedAt,
    int DepartmentCount);
