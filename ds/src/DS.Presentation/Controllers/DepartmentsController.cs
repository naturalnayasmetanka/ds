using DS.Application.Departments.Services;
using DS.Contracts.Departments.Create;
using DS.Contracts.Departments.GetById;
using DS.Contracts.Departments.Update;
using Microsoft.AspNetCore.Mvc;

namespace DS.Presentation.Controllers;

[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmantsService _departmantsService;

    public DepartmentsController(IDepartmantsService departmantsService)
    {
        _departmantsService = departmantsService;
    }

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
        var createDepartmentResult =
            await _departmantsService.CreateAsync(request, cancellationToken);

        if (createDepartmentResult.IsFailure)
            return BadRequest(createDepartmentResult.Error);

        return Ok(createDepartmentResult.Value);
    }

    [HttpPatch("departments/{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var updateDepartmentResult =
            await _departmantsService.UpdateAsync(id, request, cancellationToken);

        if (updateDepartmentResult.IsFailure)
            return BadRequest(updateDepartmentResult.Error);

        return Ok(updateDepartmentResult.Value);
    }

    [HttpDelete("departments/{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }
}
