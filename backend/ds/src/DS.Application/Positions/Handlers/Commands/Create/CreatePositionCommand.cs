using DS.Application.Abstractions.Handlers;
using DS.Contracts.Positions.Create;

namespace DS.Application.Positions.Handlers.Commands.Create;

public record CreatePositionCommand(CreatePositionRequest request) : ICommand;
