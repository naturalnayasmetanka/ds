using DS.Application.DepartmentsLocations.Services;
using DS.Contracts.DepartmentsLocations.Bind;
using DS.Contracts.DepartmentsLocations.Unbind;
using Microsoft.AspNetCore.Mvc;

namespace DS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsLocationsController : ControllerBase
    {
        private readonly IDepartmentLocationsService _departmentLocationsService;

        public DepartmentsLocationsController(
            IDepartmentLocationsService departmentLocationsService)
        {
            _departmentLocationsService = departmentLocationsService;
        }

        [HttpPost("/departments/{departmentId:guid}/locations/{locationId:guid}")]
        public async Task<IActionResult> Create(
              [FromRoute] Guid departmentId,
              [FromRoute] Guid locationId,
              CancellationToken cancellationToken)
        {
            var createDepartmentResult =
                await _departmentLocationsService
                .BindAsync(new BindDepartmentLocationRequest(departmentId, locationId), cancellationToken);

            return Ok(createDepartmentResult);
        }

        [HttpDelete("/departments/{departmentId:guid}/locations/{locationId:guid}")]
        public async Task<IActionResult> Delete(
              [FromRoute] Guid departmentId,
              [FromRoute] Guid locationId,
              CancellationToken cancellationToken)
        {
            var createDepartmentResult =
                await _departmentLocationsService
                .UnbindAsync(new UnbindDepartmentLocationRequest(departmentId, locationId), cancellationToken);

            return Ok(createDepartmentResult);
        }
    }
}
