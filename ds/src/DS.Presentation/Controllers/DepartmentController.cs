using DS.Contracts.Department.Create;
using DS.Contracts.Department.GetById;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DS.Presentation.Controllers;

[ApiController]
public class DepartmentController : ControllerBase
{
    [HttpGet("departments")]
    [SwaggerOperation(Tags = ["Department"])]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }

    [HttpGet("departments/{id:guid}")]
    [SwaggerOperation(Tags = ["Department"])]
    public async Task<IActionResult> GetById([FromRoute]GetDepartmentByIdRequest request)
    {
        return Ok(Guid.NewGuid());
    }

    [HttpPost("departments")]
    [SwaggerOperation(Tags = ["Department"])]
    public async Task<IActionResult> Create([FromBody]CreateDepartmentRequest request)
    {
        return Ok("Department");
    }
}
