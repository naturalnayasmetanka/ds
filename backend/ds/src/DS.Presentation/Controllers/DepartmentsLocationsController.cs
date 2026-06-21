using DS.Application.Abstractions.Handlers;
using DS.Application.DepartmentsLocations.Handlers.Bind;
using DS.Application.DepartmentsLocations.Handlers.Unbind;
using DS.Contracts.DepartmentsLocations.Bind;
using DS.Contracts.DepartmentsLocations.Unbind;
using Microsoft.AspNetCore.Mvc;

namespace DS.Presentation.Controllers
{
    [Route("departments-locations")]
    public class DepartmentsLocationsController : ControllerBase
    {
        [HttpPost("/departments/{departmentId:guid}/locations/{locationId:guid}")]
        public async Task<IActionResult> Create(
              [FromServices] ICommandHandler<BindDepartmentLocationCommand> handler,
              [FromRoute] Guid departmentId,
              [FromRoute] Guid locationId,
              CancellationToken cancellationToken)
        {
            var command = new BindDepartmentLocationCommand(new BindDepartmentLocationRequest(departmentId, locationId));

            var createDLResult = await handler.Handle(command, cancellationToken);

            if (createDLResult.IsFailure)
                return BadRequest(createDLResult.Error);

            return Ok(createDLResult);
        }

        [HttpDelete("/departments/{departmentId:guid}/locations/{locationId:guid}")]
        public async Task<IActionResult> Delete(
            [FromServices] ICommandHandler<UnBindDepartmentLocationCommand> handler,
            [FromRoute] Guid departmentId,
            [FromRoute] Guid locationId,
            CancellationToken cancellationToken)
        {
            var command = new UnBindDepartmentLocationCommand(new UnbindDepartmentLocationRequest(departmentId, locationId));

            var createDLResult = await handler.Handle(command, cancellationToken);

            if (createDLResult.IsFailure)
                return BadRequest(createDLResult.Error);

            return Ok(createDLResult);
        }
    }
}
