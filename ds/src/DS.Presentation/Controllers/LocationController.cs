using DS.Contracts.Location.Create;
using DS.Contracts.Location.GetById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DS.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    [HttpGet("locations")]
    [SwaggerOperation(Tags = ["Location"])]
    public async Task<IActionResult> Get()
    {
        return Ok("Location");
    }

    [HttpGet("locations/{id:guid}")]
    [SwaggerOperation(Tags = ["Location"])]
    public async Task<IActionResult> GetById([FromRoute] GetLocationByIdRequest request)
    {
        return Ok("Location");
    }

    [HttpPost("locations")]
    [SwaggerOperation(Tags = ["Location"])]
    public async Task<IActionResult> Create([FromBody] CreateLocationRequest request)
    {
        return Ok("Location");
    }
}
