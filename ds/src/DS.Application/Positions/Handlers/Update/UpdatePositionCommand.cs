using DS.Application.Abstractions.Handlers;
using DS.Contracts.Positions.Update;

namespace DS.Application.Positions.Handlers.Update;

public record UpdatePositionCommand(Guid Id, UpdatePositionRequest request) : ICommand;
