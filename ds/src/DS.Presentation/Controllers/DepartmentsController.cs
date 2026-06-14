using DS.Application.Abstractions.Handlers;
using DS.Application.Departments.Handlers.Commands.Create;
using DS.Application.Departments.Handlers.Commands.Update;
using DS.Application.Departments.Handlers.Commands.Delete;
using DS.Application.Departments.Handlers.Queries.GetBy;
using DS.Application.Departments.Handlers.Queries.GetList;
using DS.Application.DepartmentsPositions.Handlers.Bind;
using DS.Application.DepartmentsPositions.Handlers.Unbind;
using DS.Contracts.Common;
using DS.Contracts.Departments.Create;
using DS.Contracts.Departments.Get;
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
        [FromServices] IQueryHandler<PagedResult<DepartmentListItemDto>, GetDepartmentsListQuery> handler,
        [FromQuery] GetDepartmentsListRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = new GetDepartmentsListQuery(request);
        var getPagedListResult = await handler.Handle(query, cancellationToken);

        if (getPagedListResult.IsFailure)
            return BadRequest(getPagedListResult.Error);

        return Ok(getPagedListResult.Value);
    }

    [HttpGet("departments/{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] GetDepartmentRequest request,
        [FromServices] IQueryHandler<GetDepartmentResponse?, GetDepartmentQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetDepartmentQuery(request);
        var getDepartmentResult = await handler.Handle(query, cancellationToken);

        if (getDepartmentResult.IsFailure)
            return NotFound(getDepartmentResult.Error);

        return Ok(getDepartmentResult.Value);
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
        [FromServices] ICommandHandler<DeleteDepartmentCommand> handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteDepartmentCommand(id);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }

    [HttpPost("/departments/{departmentId:guid}/positions/{positionId:guid}")]
    public async Task<IActionResult> BindPosition(
        [FromServices] ICommandHandler<BindDepartmentPositionCommand> handler,
        [FromRoute] Guid departmentId,
        [FromRoute] Guid positionId,
        CancellationToken cancellationToken)
    {
        var command = new BindDepartmentPositionCommand(new DS.Contracts.DepartmentsPositions.Bind.BindDepartmentPositionRequest(departmentId, positionId));

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result);
    }

    [HttpDelete("/departments/{departmentId:guid}/positions/{positionId:guid}")]
    public async Task<IActionResult> UnbindPosition(
        [FromServices] ICommandHandler<UnbindDepartmentPositionCommand> handler,
        [FromRoute] Guid departmentId,
        [FromRoute] Guid positionId,
        CancellationToken cancellationToken)
    {
        var command = new UnbindDepartmentPositionCommand(new DS.Contracts.DepartmentsPositions.Unbind.UnbindDepartmentPositionRequest(departmentId, positionId));

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result);
    }
}
