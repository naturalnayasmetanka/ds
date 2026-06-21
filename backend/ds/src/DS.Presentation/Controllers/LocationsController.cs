using DS.Application.Abstractions.Handlers;
using DS.Application.Locations.Handlers.Commands.Create;
using DS.Application.Locations.Handlers.Commands.Update;
using DS.Application.Locations.Handlers.Queries.Get;
using DS.Application.Locations.Handlers.Queries.GetAll;
using DS.Application.Locations.Handlers.Queries.GetTop;
using DS.Contracts.Locations.Create;
using DS.Contracts.Locations.Get;
using DS.Contracts.Locations.GetById;
using DS.Contracts.Locations.GetTop;
using DS.Contracts.Locations.Update;
using Microsoft.AspNetCore.Mvc;

namespace DS.Presentation.Controllers;

[Route("locations")]
public class LocationsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromServices]IQueryHandler<List<GetLocationsResponse>, GetAllLocationsQuery> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetAllLocationsQuery(), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("top")]
    public async Task<IActionResult> GetTop(
        [FromServices] IQueryHandler<List<GetTopResponse>, UnitQuery> handler,
        CancellationToken cancellationToken)
    {
        var getTopLocationsResult = await handler.Handle(new UnitQuery(), cancellationToken);

        if (getTopLocationsResult.IsFailure)
            return NotFound(getTopLocationsResult.Error);

        return Ok(getTopLocationsResult.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] GetLocationRequest request,
        [FromServices] IQueryHandler<GetLocationResponse?, GetLocationQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetLocationQuery(request);
        var getLocationResult = await handler.Handle(query, cancellationToken);

        if (getLocationResult.IsFailure)
            return NotFound(getLocationResult.Error);

        return Ok(getLocationResult.Value);
    }

    [HttpPost]
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

    [HttpPatch("{id:guid}")]
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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }
}
