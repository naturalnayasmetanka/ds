using DS.Application.Locations.Services;
using DS.Contracts.Locations.Create;
using DS.Contracts.Locations.GetById;
using DS.Contracts.Locations.Update;
using Microsoft.AspNetCore.Mvc;

namespace DS.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{
    private readonly ILocationsService _locationsService;

    public LocationsController(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

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
        [FromBody] CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var createLocationResult =
            await _locationsService.CreateAsync(request, cancellationToken);

        if (createLocationResult.IsFailure)
            return BadRequest(createLocationResult.Error);

        return Ok(createLocationResult.Value);
    }

    [HttpPatch("locations/{id:guid}")]
    public async Task<IActionResult> Update(
       [FromRoute] Guid id,
       [FromBody] UpdateLocationRequest request,
       CancellationToken cancellationToken)
    {
        var updateLocationResult =
            await _locationsService.UpdateAsync(id, request, cancellationToken);

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
