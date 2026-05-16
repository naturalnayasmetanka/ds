using DS.Contracts.Position.Create;
using DS.Contracts.Position.GetById;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DS.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]

public class PositionController : ControllerBase
{
    [HttpGet("positions")]
    [SwaggerOperation(Tags = ["Position"])]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }

    [HttpGet("positions/{id:guid}")]
    [SwaggerOperation(Tags = ["Position"])]
    public async Task<IActionResult> GetById([FromRoute] GetPositionByIdRequest request)
    {
        return Ok("Position");
    }

    [HttpPost("positions")]
    [SwaggerOperation(Tags = ["Position"])]
    public async Task<IActionResult> Create([FromBody] CreatePositionRequest request)
    {
        return Ok("Position");
    }
}
