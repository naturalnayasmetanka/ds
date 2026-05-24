using DS.Application.Departments.Services;
using DS.Contracts.Department.Create;
using DS.Contracts.Department.GetById;
using DS.Contracts.Department.Update;
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
            await _departmantsService
            .CreateAsync(request, cancellationToken);

        return Ok(createDepartmentResult);
    }

    [HttpPatch("departments/{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var updateDepartmentResult =
            await _departmantsService
            .UpdateAsync(id, request, cancellationToken);

        return Ok(updateDepartmentResult);
    }

    [HttpDelete("departments/{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }
}
