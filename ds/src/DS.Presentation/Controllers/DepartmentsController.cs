using DS.Application.Abstractions;
using DS.Application.Departments.Handlers.Create;
using DS.Application.Departments.Handlers.Update;
using DS.Contracts.Departments.Create;
using DS.Contracts.Departments.GetById;
using DS.Contracts.Departments.Update;
using DS.Presentation.Results;
using Microsoft.AspNetCore.Mvc;

namespace DS.Presentation.Controllers;

[ApiController]
public class DepartmentsController : ControllerBase
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
    public async Task<EndpointResult<Guid>> Create(
        [FromServices] ICommandHandler<Guid, CreateDepartmentCommand> handler,
        [FromBody] CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateDepartmentCommand(request);

        var createDepartmentResult = await handler.Handle(command, cancellationToken);

        return createDepartmentResult;
    }

    [HttpPatch("departments/{id:guid}")]
    public async Task<EndpointResult<Guid>> Update(
        [FromServices] ICommandHandler<Guid, UpdateDepartmentCommand> handler,
        [FromRoute] Guid id,
        [FromBody] UpdateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateDepartmentCommand(id, request);

        var updateDepartmentResult = await handler.Handle(command, cancellationToken);

        return updateDepartmentResult;
    }

    [HttpDelete("departments/{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }
}
