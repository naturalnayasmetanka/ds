using DS.Contracts.Location.Create;
using DS.Contracts.Location.GetById;
using DS.Contracts.Location.Update;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DS.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    [HttpGet("locations")]
    [SwaggerOperation(Tags = ["Location"])]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        return Ok("Location");
    }

    [HttpGet("locations/{id:guid}")]
    [SwaggerOperation(Tags = ["Location"])]
    public async Task<IActionResult> GetById(
        [FromRoute] GetLocationByIdRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(Guid.NewGuid());
    }

    [HttpPost("locations")]
    [SwaggerOperation(Tags = ["Location"])]
    public async Task<IActionResult> Create(
        [FromBody] CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        return Created();
    }

    [HttpPut("locations/{id:guid}")]
    [SwaggerOperation(Tags = ["Location"])]
    public async Task<IActionResult> Update(
       [FromRoute] Guid id,
       [FromBody] UpdateLocationRequest request,
       CancellationToken cancellationToken)
    {
        return Ok("Location");
    }

    [HttpDelete("locations/{id:guid}")]
    [SwaggerOperation(Tags = ["Location"])]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }
}
