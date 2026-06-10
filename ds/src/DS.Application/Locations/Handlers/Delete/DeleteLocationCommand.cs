using DS.Application.Abstractions.Handlers;

namespace DS.Application.Locations.Handlers.Delete;

public record DeleteLocationCommand(Guid Id) : ICommand;
