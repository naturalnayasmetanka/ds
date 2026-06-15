using DS.Application.Abstractions.Handlers;
using DS.Contracts.Locations.Update;

namespace DS.Application.Locations.Handlers.Commands.Update;

public record UpdateLocationCommand(Guid locationId,UpdateLocationRequest request) : ICommand;