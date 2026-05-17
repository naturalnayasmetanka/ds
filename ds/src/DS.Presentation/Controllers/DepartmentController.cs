using DS.Contracts.Department.Create;
using DS.Contracts.Department.GetById;
using DS.Contracts.Department.Update;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DS.Presentation.Controllers;

[ApiController]
public class DepartmentController : ControllerBase
{
    [HttpGet("departments")]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet("departments/{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] GetDepartmentByIdRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(Guid.NewGuid());
    }

    [HttpPost("departments")]
    public async Task<IActionResult> Create(
        [FromBody] CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        return Created();
    }

    [HttpPut("departments/{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        return Ok("Department");
    }

    [HttpDelete("departments/{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }
}
