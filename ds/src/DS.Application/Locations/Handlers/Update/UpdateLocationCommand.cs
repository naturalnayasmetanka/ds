using DS.Application.Abstractions;
using DS.Contracts.Locations.Update;

namespace DS.Application.Locations.Handlers.Update;

public record UpdateLocationCommand(Guid locationId,UpdateLocationRequest request) : ICommand;