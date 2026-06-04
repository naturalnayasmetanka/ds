using DS.Application.Abstractions;
using DS.Application.Locations.Handlers.Create;
using DS.Application.Locations.Handlers.Update;
using DS.Contracts.Locations.Create;
using DS.Contracts.Locations.GetById;
using DS.Contracts.Locations.Update;
using Microsoft.AspNetCore.Mvc;

namespace DS.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{
    [HttpGet("locations")]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        return Ok("Location");
    }

    [HttpGet("locations/{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] GetLocationByIdRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(Guid.NewGuid());
    }

    [HttpPost("locations")]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<Guid, CreateLocationCommand> handler,
        [FromBody] CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateLocationCommand(request);

        var createLocationResult = await handler.Handle(command, cancellationToken);

        if (createLocationResult.IsFailure)
            return BadRequest(createLocationResult.Error);

        return Ok(createLocationResult.Value);
    }

    [HttpPatch("locations/{id:guid}")]
    public async Task<IActionResult> Update(
        [FromServices] ICommandHandler<Guid, UpdateLocationCommand> handler,
        [FromRoute] Guid id,
        [FromBody] UpdateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLocationCommand(id, request);

        var updateLocationResult = await handler.Handle(command, cancellationToken);

        if (updateLocationResult.IsFailure)
            return BadRequest(updateLocationResult.Error);

        return Ok(updateLocationResult.Value);
    }

    [HttpDelete("locations/{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }
}
