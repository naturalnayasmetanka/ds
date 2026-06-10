using DS.Application.Abstractions.Handlers;

namespace DS.Application.Locations.Handlers.Commands.Delete;

public record DeleteLocationCommand(Guid Id) : ICommand;
