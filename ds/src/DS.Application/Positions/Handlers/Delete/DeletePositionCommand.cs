using DS.Application.Abstractions.Handlers;
using DS.Contracts.Positions.Delete;

namespace DS.Application.Positions.Handlers.Delete;

public record DeletePositionCommand(Guid Id) : ICommand;
