using DS.Application.Abstractions.Handlers;
using DS.Application.Positions.Handlers.Commands.Create;
using DS.Application.Positions.Handlers.Commands.Delete;
using DS.Application.Positions.Handlers.Commands.Update;
using DS.Contracts.Positions.Create;
using DS.Contracts.Positions.Update;
using DS.Presentation.Results;
using Microsoft.AspNetCore.Mvc;

namespace DS.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]

public class PositionsController : ControllerBase
{
    [HttpPost("positions")]
    public async Task<EndpointResult<Guid>> Create(
        [FromServices] ICommandHandler<Guid, CreatePositionCommand> handler,
        [FromBody] CreatePositionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePositionCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        return result;
    }

    [HttpPatch("positions/{id:guid}")]
    public async Task<EndpointResult<Guid>> Update(
       [FromServices] ICommandHandler<Guid, UpdatePositionCommand> handler,
       [FromRoute] Guid id,
       [FromBody] UpdatePositionRequest request,
       CancellationToken cancellationToken)
    {
        var command = new UpdatePositionCommand(id, request);

        var result = await handler.Handle(command, cancellationToken);

        return result;
    }

    [HttpDelete("positions/{id:guid}")]
    public async Task<EndpointResult<Guid>> Delete(
        [FromServices] ICommandHandler<Guid, DeletePositionCommand> handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeletePositionCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        return result;
    }
}
