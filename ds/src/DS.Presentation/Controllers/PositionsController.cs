using DS.Contracts.Position.Create;
using DS.Contracts.Position.GetById;
using DS.Contracts.Position.Update;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DS.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]

public class PositionsController : ControllerBase
{
    [HttpGet("positions")]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet("positions/{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] GetPositionByIdRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(Guid.NewGuid());
    }

    [HttpPost("positions")]
    public async Task<IActionResult> Create(
        [FromBody] CreatePositionRequest request,
        CancellationToken cancellationToken)
    {
        return Created();
    }

    [HttpPut("positions/{id:guid}")]
    public async Task<IActionResult> Update(
       [FromRoute] Guid id,
       [FromBody] UpdatePositionRequest request,
       CancellationToken cancellationToken)
    {
        return Ok("Position");
    }

    [HttpDelete("positions/{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }
}
