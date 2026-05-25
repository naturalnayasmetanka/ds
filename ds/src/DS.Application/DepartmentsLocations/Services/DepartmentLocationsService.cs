using DS.Application.Departments.Repositories;
using DS.Application.DepartmentsLocations.Repositories;
using DS.Application.Locations.Repositories;
using DS.Contracts.DepartmentsLocations.Bind;
using DS.Contracts.DepartmentsLocations.Unbind;
using DS.Domain.Models.DepartmentsLocations;
using Microsoft.Extensions.Logging;

namespace DS.Application.DepartmentsLocations.Services;

public class DepartmentLocationsService : IDepartmentLocationsService
{
    private readonly IDepartmentsLocationsRepository _departmentsLocationsRepository;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ILocationsRepository _locationsRepository;
    private readonly ILogger<DepartmentLocationsService> _logger;

    public DepartmentLocationsService(
        IDepartmentsLocationsRepository departmentsLocationsRepository,
        IDepartmentsRepository departmentsRepository,
        ILocationsRepository locationsRepository,
        ILogger<DepartmentLocationsService> logger)
    {
        _departmentsLocationsRepository = departmentsLocationsRepository;
        _departmentsRepository = departmentsRepository;
        _locationsRepository = locationsRepository;
        _logger = logger;
    }

    public async Task<(Guid, Guid)?> BindAsync(
        BindDepartmentLocationRequest request,
        CancellationToken cancellationToken)
    {
        var department = await _departmentsRepository
            .GetByFieldAsync(x => x.Id == request.departmentId, cancellationToken);

        if (department is null)
            throw new Exception($"departmentId is null");

        var location = await _locationsRepository
            .GetByFieldAsync(x => x.Id == request.locationId, cancellationToken);

        if (department is null)
            throw new Exception($"locationId is null");

        var departmentLocation = await _departmentsLocationsRepository
            .GetByIdsAsync(DepartmentLocation.Create(request.departmentId, request.locationId), cancellationToken);

        if (departmentLocation is not null)
            throw new Exception($"departmentLocation already exists");

        await _departmentsLocationsRepository
            .BindAsync(DepartmentLocation.Create(request.departmentId, request.locationId), cancellationToken);

        await _departmentsLocationsRepository.SaveAsync(cancellationToken);

        return (request.departmentId, request.locationId);
    }

    public async Task<(Guid, Guid)?> UnbindAsync(
        UnbindDepartmentLocationRequest request,
        CancellationToken cancellationToken)
    {
        var department = await _departmentsRepository.
            GetByFieldAsync(x => x.Id == request.departmentId, cancellationToken);

        if (department is null)
            throw new Exception($"departmentId is null");

        var location = await _locationsRepository
            .GetByFieldAsync(x => x.Id == request.locationId, cancellationToken);

        if (department is null)
            throw new Exception($"locationId is null");

        var departmentLocation = await _departmentsLocationsRepository
            .GetByIdsAsync(DepartmentLocation.Create(request.departmentId, request.locationId), cancellationToken);

        if (departmentLocation is null)
            throw new Exception($"departmentLocation is null");

        _departmentsLocationsRepository
            .UnbindAsync(DepartmentLocation.Create(request.departmentId, request.locationId), cancellationToken);

        await _departmentsLocationsRepository.SaveAsync(cancellationToken);

        return (request.departmentId, request.locationId);
    }
}
